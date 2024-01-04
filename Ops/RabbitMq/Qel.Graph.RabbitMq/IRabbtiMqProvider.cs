namespace Qel.Graph.RabbitMq;

public interface IRabbtiMqProvider
{
    public void Produce(string message, string queueName, string routingKey, string exchangeName);

    public Task<string?> Consume(string queueName);
}
