using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Qel.Graph.Web.Processing;

public class RabbitMqProvider
{
    public void Produce(string message, string queueName, string routingKey, string exchangeName) 
    {
        var channel = Connect(queueName);

        var body = Encoding.UTF8.GetBytes(message);

        channel.BasicPublish(exchange: exchangeName,
                           routingKey: routingKey,
                           basicProperties: null,
                           body: body);
    }

    public async Task<string?> Consume(string queueName) 
    {
        var channel = Connect(queueName);

        var consumer = new EventingBasicConsumer(channel);

        var tcs = new TaskCompletionSource<string>();
        consumer.Received += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            tcs.TrySetResult(message);
        };
        channel.BasicConsume(queue: queueName,
                           autoAck: true,
                           consumer: consumer);
        var result = await tcs.Task;
        return result;
    }

    private static IModel Connect(string queueName)
    {
        var factory = new ConnectionFactory() { HostName = "localhost", Port = 5672 }; // замените localhost на адрес вашего сервера RabbitMQ
        var connection = factory.CreateConnection();
        var channel = connection.CreateModel();

        channel.QueueDeclare(queue: queueName,
                           durable: true,
                           exclusive: false,
                           autoDelete: false,
                           arguments: null);
        return channel;
    }

    
}
