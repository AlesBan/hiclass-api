using HiClass.Domain.Entities.Notifications;
using HiClass.Domain.Enums;

namespace HiClass.Application.Models.Notifications;

public class NotificationResponseDto
{
    public string NotificationType { get; set; } = string.Empty;
    public NotificationMessage Message { get; set; } = new();
    public bool IsRead { get; set; }
    public DateTime CreatedAt { get; set; }
}