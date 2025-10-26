using Ecommece.Core.Models;
using Ecommece.Core.Shipping;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

var factory = new ConnectionFactory() { HostName = "localhost" };
using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

channel.QueueDeclare(queue: "shipping_requests", durable: false, exclusive: false, autoDelete: false, arguments: null);

Console.WriteLine("🚚 Waiting for shipping requests...");

var consumer = new EventingBasicConsumer(channel);
consumer.Received += (model, ea) =>
{
    var body = ea.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    var shippingOrder = JsonConvert.DeserializeObject<ShippingOrder>(message);

    Console.WriteLine($"📦 Processing shipment for Order #{shippingOrder.OrderId} using {shippingOrder.ShippingProvider}");

    var provider = ShippingFactory.Create(shippingOrder.ShippingProvider);
    var trackingNumber = provider.ShipOrder(shippingOrder.OrderId);

    shippingOrder.TrackingNumber = trackingNumber;
    shippingOrder.Status = "Shipped";

    Console.WriteLine($"✅ Order #{shippingOrder.OrderId} shipped via {provider.Name}, Tracking: {trackingNumber}");
};

channel.BasicConsume(queue: "shipping_requests", autoAck: true, consumer: consumer);

Console.ReadLine();

