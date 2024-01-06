namespace Qel.Graph.RabbitMq.Configuration;

/// <summary>
/// Конфигурация RabbitMQ.
/// </summary>
public class RabbitMQConfiguration
{
    /// <summary>
    /// Список endpoints.
    /// </summary>
    public IList<RabbitMQEndpoint> Endpoints { get; set; }

    /// <summary>
    /// Список маршрутов.
    /// </summary>
    public IList<RabbitMQRoute> Routes { get; set; }

    /// <summary>
    /// Имя пользователя.
    /// </summary>
    public string UserName { get; set; }

    /// <summary>
    /// Пароль.
    /// </summary>
    public string Password { get; set; }

    /// <summary>
    /// Имя очереди.
    /// </summary>
    public string QueueName { get; set; }
}