using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;

namespace HiClass.Infrastructure.Services.NotificationHandlerService;

public class NotificationHandlerService : INotificationHandlerService
{

    private readonly IConfiguration _configuration;

    public NotificationHandlerService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void SendMessage<T>(T message)
    {
        var factory = new ConnectionFactory
        {
            HostName = _configuration["RABBITMQ_DEFAULT_HOSTNAME"],
            UserName = _configuration["RABBITMQ_DEFAULT_USERNAME"],
            Password = _configuration["RABBITMQ_DEFAULT_PASSWORD"],
            VirtualHost = "/"
        };

        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();
        channel.QueueDeclare(queue: "notifications", durable: false, exclusive: false, autoDelete: false,
            arguments: null);
        var json = Newtonsoft.Json.JsonConvert.SerializeObject(message);
        var body = System.Text.Encoding.UTF8.GetBytes(json);
        channel.BasicPublish(exchange: "", routingKey: "notifications", basicProperties: null, body: body);
    }
    
    public void ScheduleMessage<T>(T message, DateTime deliveryTime)
    {
        var factory = new ConnectionFactory
        {
            HostName = _configuration["RABBITMQ_DEFAULT_HOSTNAME"],
            UserName = _configuration["RABBITMQ_DEFAULT_USERNAME"],
            Password = _configuration["RABBITMQ_DEFAULT_PASSWORD"],
            VirtualHost = "/"
        };

        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();

        // Устанавливаем плагин RabbitMQ Delayed Message
        var arguments = new Dictionary<string, object>
        {
            { "x-delayed-type", "direct" }
        };

        // Создаем очередь для отложенных сообщений
        channel.QueueDeclare(queue: "delayed_notifications", durable: false, exclusive: false, autoDelete: false,
            arguments: arguments);

        channel.ExchangeDeclare(exchange: "delayed_exchange", type: "x-delayed-message", durable: false, autoDelete: false, arguments: arguments);
        channel.QueueBind(queue: "delayed_notifications", exchange: "delayed_exchange", routingKey: "");

        var now = DateTime.Now;

        // Вычисляем время доставки сообщения
        var delay = deliveryTime - now;

        if (delay.TotalMilliseconds < 0)
        {
            delay = TimeSpan.Zero;
        }
    
        // Создаем заголовок сообщения с указанием времени доставки
        var headers = new Dictionary<string, object>
        {
            { "x-delay", 8000 }
        };

        var properties = channel.CreateBasicProperties();
        properties.Headers = headers;

        var json = Newtonsoft.Json.JsonConvert.SerializeObject(message);
        var body = System.Text.Encoding.UTF8.GetBytes(json);

        // Отправляем сообщение в очередь для отложенных сообщений
        channel.BasicPublish(exchange: "delayed_exchange", routingKey: "", basicProperties: properties, body: body);
    }
}