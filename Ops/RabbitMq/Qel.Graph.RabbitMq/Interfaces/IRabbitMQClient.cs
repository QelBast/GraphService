using Qel.Graph.RabbitMq.Configuration;

namespace Qel.Graph.RabbitMq.Interfaces;

/// <summary>
/// Интерфейс отправки для работы с RabbitMQ
/// </summary>
public interface IRabbitMQClient<TConfig> where TConfig : RabbitMQConfiguration
{
    /// <summary>
    /// Отправить сообщение
    /// </summary>
    void SendJson<T>(string routeName, T data, object? headers = null);

    /// <summary>
    /// Отправить сообщение в JSON формате.
    /// </summary>
    void SendJson<T>(string routeName, T data, IDictionary<string, object>? headers = null);

    /// <summary>
    /// Отправить сообщение в XML формате.
    /// </summary>
    void SendXml<T>(string routeName, T data, IDictionary<string, object>? headers = null);
    /// <summary>
    /// Отправить данные
    /// </summary>
    void Send(string routeName, byte[] data, IDictionary<string, object>? headers = null);

    /// <summary>
    /// Отправить данные
    /// </summary>
    void Send(string exchange, string routingKey, byte[] data, IDictionary<string, object>? headers = null);
}