using RabbitMQ.Client;

namespace HiClass.Infrastructure.Services.NotificationHandlerService;

public interface INotificationHandlerService
{
    public void SendMessage<T>(T message);
    public void ScheduleMessage<T>(T message, DateTime deliveryTime);
}