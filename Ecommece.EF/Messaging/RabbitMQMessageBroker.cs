using Ecommece.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommece.EF.Messaging
{
    public class RabbitMQMessageBroker : IMessageBroker
    {
       // private readonly ConnectionFactory _factory;

        public RabbitMQMessageBroker()
        {
            //_factory = new ConnectionFactory()
            //{
            //    HostName = "localhost",
            //    UserName = "guest",
            //    Password = "guest"
            //};
        }

        public Task PublishAsync(string topic, object message)
        {
            //using var connection = _factory.CreateConnection();
            //using var channel = connection.CreateModel();

            //channel.QueueDeclare(queue: topic, durable: false, exclusive: false, autoDelete: false, arguments: null);

            //var json = JsonConvert.SerializeObject(message);
            //var body = Encoding.UTF8.GetBytes(json);

            //channel.BasicPublish(exchange: "", routingKey: topic, basicProperties: null, body: body);

            //Console.WriteLine($"[x] Published to {topic}: {json}");
            return Task.CompletedTask;
        }
    }
}
