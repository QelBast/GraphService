namespace Qel.Graph.RabbitMq.Configuration;

/// <summary>
/// RabbitMQ Endpoint
/// </summary>
public class RabbitMQEndpoint
{
    /// <summary>
    /// Имя хоста
    /// </summary>
    public string HostName { get; set; }

    /// <summary>
    /// Порт
    /// </summary>
    public int Port { get; set; }
}