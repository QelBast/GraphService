namespace Qel.Graph.RabbitMq.Configuration;

/// <summary>
/// Маршрут RabbitMQ
/// </summary>
public class RabbitMQRoute
{
    /// <summary>
    /// Название маршрута
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Exchange
    /// </summary>
    public string Exchange { get; set; }

    /// <summary>
    /// Ключ маршрутизации
    /// </summary>
    public string RoutingKey { get; set; }
}
