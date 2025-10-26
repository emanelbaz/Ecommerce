using Ecommece.Core.Models;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Net;
using System.Net.Mail;
using System.Text;

var factory = new ConnectionFactory()
{
    HostName = "localhost",
    UserName = "guest",
    Password = "guest"
};

using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

channel.QueueDeclare(queue: "email_notifications", durable: false, exclusive: false, autoDelete: false, arguments: null);

Console.WriteLine("📨 Waiting for email messages...");

var consumer = new EventingBasicConsumer(channel);
consumer.Received += async (model, ea) =>
{
    var body = ea.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);

    var emailData = JsonConvert.DeserializeObject<EmailMessage>(message);

    Console.WriteLine($"📧 Preparing email for order {emailData.OrderId}...");

    try
    {
        using var smtp = new SmtpClient("smtp.gmail.com", 587)
        {
            Credentials = new NetworkCredential("your_email@gmail.com", "your_app_password"),
            EnableSsl = true
        };

        var mail = new MailMessage("your_email@gmail.com", emailData.To)
        {
            Subject = emailData.Subject,
            Body = emailData.Body
        };

        await smtp.SendMailAsync(mail);

        Console.WriteLine($"✅ Email sent successfully to {emailData.To}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"❌ Failed to send email: {ex.Message}");
    }
};

channel.BasicConsume(queue: "email_notifications", autoAck: true, consumer: consumer);

Console.ReadLine();
