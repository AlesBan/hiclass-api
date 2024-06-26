using HiClass.Domain.Entities.Notifications;
using HiClass.Domain.Enums;

namespace HiClass.Application.Models.Notifications;

public class NotificationDto
{
    public NotificationType NotificationType { get; set; }
    public NotificationMessage NotificationMessage { get; set; }
    public Guid UserReceiverId { get; set; }
}