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
optionsBuilder.UseSqlServer("Server=62.117.61.133;Database=EcommeceDB;User Id=sa;Password=33355555@dts;TrustServerCertificate=True;");

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
            // Publish Email Event
            var emailMessage = new EmailMessage
            {
                OrderId = order.Id,
                To = order.BuyerEmail, // assuming navigation property
                Subject = "Order Payment Confirmation",
                Body = $"Your order #{order.Id} has been paid successfully."
            };

            var emailJson = JsonConvert.SerializeObject(emailMessage);
            var emailBody = Encoding.UTF8.GetBytes(emailJson);

            channel.BasicPublish(exchange: "", routingKey: "email_notifications", basicProperties: null, body: emailBody);

            Console.WriteLine($"📩 Sent email notification for Order {order.Id}");

            // Shipping event
            var shippingOrder = new ShippingOrder
            {
                OrderId = order.Id,
                ShippingProvider = "DHL"
            };

            var shippingJson = JsonConvert.SerializeObject(shippingOrder);
            var shippingBody = Encoding.UTF8.GetBytes(shippingJson);

            channel.BasicPublish(exchange: "", routingKey: "shipping_requests", basicProperties: null, body: shippingBody);

            Console.WriteLine($"🚚 Shipping request sent for Order {order.Id}");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"⚠️ Error: {ex.Message}");
    }
};

channel.BasicConsume(queue: "payments", autoAck: true, consumer: consumer);

Console.ReadLine();
