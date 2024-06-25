using HiClass.Application.Models.Notifications;
using Microsoft.Extensions.Configuration;

namespace HiClass.Infrastructure.IntegrationServices.Firebase.FireBaseNotificationSender;

public interface IFireBaseNotificationSender
{
    Task SendNotificationAsync(NotificationResponseDto notificationDto, List<string> deviceTokens);
}