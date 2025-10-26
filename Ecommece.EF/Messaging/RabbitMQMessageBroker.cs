using Ecommece.Core.Interfaces;
using RabbitMQ.Client;
using System;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Ecommece.EF.Messaging
{
    public class RabbitMQMessageBroker : IMessageBroker
    {
        private readonly ConnectionFactory _factory;

        public RabbitMQMessageBroker()
        {
            _factory = new ConnectionFactory()
            {
                HostName = "localhost",
                UserName = "guest",
                Password = "guest"
            };
        }

        public Task PublishAsync(string queue, object message)
        {
            using var connection = _factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.QueueDeclare(
                queue: queue,
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );

            var json = JsonSerializer.Serialize(message);
            var body = Encoding.UTF8.GetBytes(json);

            channel.BasicPublish(
                exchange: "",
                routingKey: queue,
                basicProperties: null,
                body: body
            );

            return Task.CompletedTask;
        }
    }
}
