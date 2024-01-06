namespace Qel.Graph.ServiceBase;

/// <summary>
/// Сервис, выполняющий обработку сообщения типа T.
/// </summary>
public interface IMessageWorkService<T>
{
    /// <summary>
    /// Возвращает признак финального сервиса.
    /// </summary>
    public bool IsTerminalService { get; }

    /// <summary>
    /// Обработка сообщения.
    /// <param name="message">Сообщение для обработки.</param>
    /// </summary>
    Task<object> Work(T message);
}