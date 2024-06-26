using System.ComponentModel.DataAnnotations;
using HiClass.Domain.Entities.Main;
using HiClass.Domain.Enums;

namespace HiClass.Domain.Entities.Notifications;

public class Notification
{
    public Guid NotificationId { get; set; }
    public string NotificationType { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public bool IsRead { get; set; }
    public DateTime CreatedAt { get; set; }
    public Guid UserReceiverId { get; set; }
    public User UserReceiver { get; set; }
}