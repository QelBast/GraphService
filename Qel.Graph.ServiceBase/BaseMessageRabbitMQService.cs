using System.Reflection;
using System.Text.Json;
using System.Text;
using System.Threading.Channels;
using System.Xml.Serialization;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Qel.Graph.ServiceBase;

public class BaseMessageRabbitMQService<TInConfig, TOutConfig> : RabbitMQConsumerBase<TInConfig>
    where TInConfig : RabbitMQConfiguration
    where TOutConfig : RabbitMQConfiguration
{
    /// <summary>
    /// Настройки конфигурации входящей очереди сообщений.
    /// </summary>
    private readonly IOptions<TInConfig> _rabbitMqInOptions;

    /// <summary>
    /// Клиент для работы с RabbitMQ.
    /// </summary>
    private readonly IRabbitMQClient<TOutConfig> _rabbitMqClient;

    /// <summary>
    /// Логгер.
    /// </summary>
    private readonly ILogger<RabbitMQConsumerBase<TInConfig>> _logger;

    /// <summary>
    /// Контейнер зависимостей.
    /// </summary>
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// Guid модуля для статистики.
    /// </summary>
    private Guid StatModuleGuid { get; set; }

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="logger">Логгер.</param>
    /// <param name="options">Настройки.</param>
    /// <param name="rabbitMqClient">Клиент для работы с RabbitMQ.</param>
    /// <param name="environment"></param>
    /// <param name="serviceProvider">Контейнер зависимостей.</param>
    public BaseMessageRabbitMQService(
        ILogger<RabbitMQConsumerBase<TInConfig>> logger,
        IOptions<TInConfig> options,
        IRabbitMQClient<TOutConfig> rabbitMqClient,
        IHostEnvironment environment,
        IServiceProvider serviceProvider) : base(logger, options, environment)
    {
        _rabbitMqInOptions = options;
        _logger = logger;
        _serviceProvider = serviceProvider;
        _rabbitMqClient = rabbitMqClient;
    }

    /// <summary>
    /// Инициализация.
    /// </summary>
    protected override async Task Init()
    {
        await base.Init();

        // регистрирую модуль в таблице статистики если его там ещё нет
        // беру имя запущенной сборки и её версию
        var assembly = Assembly.GetEntryAssembly();
        string? assemblyName = assembly?.GetName().Name;
        ArgumentNullException.ThrowIfNull(assemblyName, $"У сборки {assembly?.FullName} отсутствует имя");
        Version? version = assembly?.GetName().Version;
        ArgumentNullException.ThrowIfNull(version, $"У сборки {assembly?.FullName} отсутствует версия");

        // репозиторий для работы со статистикой
        using var statisticRepository = _serviceProvider.GetRequiredService<IStatisticRepository>();
        var moduleDescription = _rabbitMqInOptions.Value.ServiceName;
        ArgumentNullException.ThrowIfNull(moduleDescription, $"У сборки {assembly?.FullName} отсутствует имя сервиса в настройках RabbitMq");
        // проверяю, был ли зарегистрирован модуль ранее
        Guid? moduleGuid = await statisticRepository.GetModuleGuid(moduleDescription, version.Major);

        if (moduleGuid is null)
        {
            // если не был - создаю запись в списке модулей 
            StatModuleGuid = Guid.NewGuid();
            await statisticRepository.AddModule(StatModuleGuid, assemblyName, version.Major, moduleDescription);
        }
        else
        {
            StatModuleGuid = moduleGuid.Value;
        }
    }

    /// <summary>
    /// Логирую сообщение в базу статистики.
    /// </summary>
    private async Task<long> LogMessageToStatisticAsync(EventMessageType eventMessageType, long? batchId)
    {
        try
        {
            using var statisticRepository = _serviceProvider.GetRequiredService<IStatisticRepository>();
            return await statisticRepository.AddEventMessage(eventMessageType, batchId, StatModuleGuid);
        }
        catch (Exception ex)
        {
            _logger.LogError("Ошибка логирования в базу данных статистики {Error}. {StackTrace}", ex.Message, ex.StackTrace);
            return -1L;
        }
    }

    /// <summary>
    /// Обработчик входящих сообщений
    /// </summary>
    protected override async Task EventReceivedHandler(object sender, BasicDeliverEventArgs ea)
    {
        using var scope = _serviceProvider.CreateScope();

        long batchId = 0;
        string? messageBody = null;
        try
        {
            // получаю тело сообщения из RabbitMQ в виде строки
            messageBody = Encoding.UTF8.GetString(ea.Body.ToArray());
            _logger.LogDebug("Получено тело RabbitMQ сообщения:\n{MessageBody}", messageBody);

            switch (_rabbitMqInOptions.Value.IncomingType)
            {
                case SerializeType.Json:
                    {
                        var serializeOptions = new JsonSerializerOptions
                        {
                            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                            WriteIndented = true
                        };
                        BaseMessage? message = JsonSerializer.Deserialize<BaseMessage>(messageBody, serializeOptions);
                        if (message == null)
                        {
                            throw new ArgumentNullException(nameof(message));
                        }
                        batchId = message.BatchId;
                        var messageService = _serviceProvider.GetRequiredService<IMessageWorkService<BaseMessage>>();

                        // выполняю обработку
                        using (var batchScope = _logger.BeginScope("Пакет {BatchId}", message.BatchId))
                        {
                            _logger.LogInformation("Обрабатываю пакет {BatchId}", message.BatchId);

                            // логирую получение пакета
                            // TODO - используйте StatModuleGuid если нужен Id текущего модуля
                            await LogMessageToStatisticAsync(EventMessageType.OpenBatch, batchId);

                            var result = await messageService.Work(message);

                            _logger.LogInformation("Пакет {BatchId} успешно обработан", batchId);

                            await LogMessageToStatisticAsync(EventMessageType.CloseBatch, batchId);

                            if (!messageService.IsTerminalService)
                            {
                                _rabbitMqClient.SendJson("success", message, null);
                                _logger.LogInformation("Сообщение об успешной обработке для пакета {BatchId} отправлено в RabbitMQ", message.BatchId);
                            }
                            else
                            {
                                _rabbitMqClient.SendXml("success", result, null);
                                _logger.LogInformation("Сообщение о результате проверки пакета документов отправлено в очередь сообщений");
                                await LogMessageToStatisticAsync(EventMessageType.ExportBatch, batchId);
                            }
                        }

                        break;
                    }
                case SerializeType.Xml:
                    {
                        TextReader textReader = new StringReader(messageBody);
                        XmlSerializer serializer = new(typeof(KnowledgeObject));
                        if (serializer.Deserialize(textReader) is not KnowledgeObject knowledgeObject)
                        {
                            Logger.LogError("Полученное сообщение не может быть десериализовано в ожидаемую объектную модель {MessageBody}", messageBody);
                            break;
                        }

                        var messageService = _serviceProvider.GetRequiredService<IMessageWorkService<KnowledgeObject>>();
                        using var batchScope = _logger.BeginScope("Получено сообщение XML {ObjectId}", knowledgeObject.ObjID);
                        var batch = (await messageService.Work(knowledgeObject)) as BaseMessage;
                        if (batch == null)
                        {
                            throw new ArgumentNullException(nameof(batch));
                        }

                        batchId = batch.BatchId;
                        _logger.LogInformation("Создан новый пакет документов {BatchId}", batch.BatchId);
                        await LogMessageToStatisticAsync(EventMessageType.CreateBatch, batch.BatchId);

                        _rabbitMqClient.SendJson("create", batch, null);
                        _logger.LogInformation("Отправка пакета документов {BatchId} на маршрут обработки", batch.BatchId);
                        await LogMessageToStatisticAsync(EventMessageType.SendBatch, batch.BatchId);

                        break;
                    }
                default:
                    throw new NotImplementedException($"Не была реализована обработка типа сообщения: {_rabbitMqInOptions.Value.IncomingType}");
            }
        }
        catch (Exception ex)
        {
            // если что-то не получилось с обработкой сообщения, логирую ошибку
            try
            {
                _logger.LogError("Ошибка при обработке сообщения: {Error}: {StackTrace}. {MessageBody}",
                    ex.Message, ex.StackTrace, messageBody);

                // отправляю сообщение дальше по маршруту "error" согласно конфигурации текущего модуля
                _rabbitMqClient.SendJson("error", new BaseErrorMessage
                {
                    BatchId = (int)batchId,
                    Exception = new()
                    {
                        Message = "Ошибка при обработке: " + ex.Message,
                        StackTrace = ex.StackTrace
                    }
                }, null);
                await LogMessageToStatisticAsync(EventMessageType.Error, batchId);
            }
            catch (Exception re)
            {
                _logger.LogError("Ошибка при уведомлении о невозможности обработке сообщения: {Error}. {StackTrace}", re.Message, re.StackTrace);
            }
        }

        Channel.BasicAck(ea.DeliveryTag, false);
    }
}
