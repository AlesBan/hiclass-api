using HiClass.Application.Models.Notifications;
using Microsoft.Extensions.Configuration;

namespace HiClass.Infrastructure.IntegrationServices.Firebase.FireBaseNotificationSender;

public interface IFireBaseNotificationSender
{
    Task SendNotificationAsync(NotificationDto notificationDto, List<string> deviceTokens);
}