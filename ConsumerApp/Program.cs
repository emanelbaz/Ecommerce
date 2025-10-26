using Ecommece.Core.Models;
using Ecommece.Core.Payments;
using Ecommece.EF.Data;
using Ecommece.EF.Messaging;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

var factory = new ConnectionFactory()
{
    HostName = "localhost",
    UserName = "guest",
    Password = "guest"
};

using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

channel.QueueDeclare(queue: "payments", durable: false, exclusive: false, autoDelete: false, arguments: null);

Console.WriteLine("💳 Waiting for payment messages...");

var optionsBuilder = new DbContextOptionsBuilder<Context>();
optionsBuilder.UseSqlServer("Server=192.168.70.100;Database=EcommeceDB;User Id=sa;Password=33355555@dts;TrustServerCertificate=True;");

var context = new Context(optionsBuilder.Options);

var consumer = new EventingBasicConsumer(channel);
consumer.Received += async (model, ea) =>
{
    var body = ea.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);

    Console.WriteLine($"[x] Received: {message}");

    try
    {
        var paymentData = JsonConvert.DeserializeObject<PaymentMessage>(message);

        // اختيار الاستراتيجية
        var paymentStrategy = PaymentStrategyFactory.GetPaymentStrategy(paymentData.PaymentMethod);

        var success = await paymentStrategy.ProcessPaymentAsync(paymentData.Amount);

        var order = await context.Orders.FirstOrDefaultAsync(o => o.Id == paymentData.OrderId);

        if (order != null)
        {
            order.Status = success ? OrderStatus.PaymentReceived : OrderStatus.PaymentReceived;
            await context.SaveChangesAsync();
        }

        Console.WriteLine(success
            ? $"✅ Payment succeeded for Order {paymentData.OrderId}"
            : $"❌ Payment failed for Order {paymentData.OrderId}");
        if (success)
        {
            var shippingMessage = new ShippingMessage
            {
                OrderId = paymentData.OrderId,
                ShippingAddress = order.ShippingAddress.ToString(),
                ShippingMethod = order.ShippingMethod
            };

            var bodyy = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(shippingMessage));

            channel.BasicPublish(
                exchange: "",
                routingKey: "shipping",
                basicProperties: null,
                body: bodyy
            );

            Console.WriteLine($"📦 Shipping requested for Order {paymentData.OrderId}");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"⚠️ Error: {ex.Message}");
    }
};

channel.BasicConsume(queue: "payments", autoAck: true, consumer: consumer);

Console.ReadLine();
