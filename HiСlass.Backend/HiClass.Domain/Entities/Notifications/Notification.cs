using System.Text.Json.Serialization;
using HiClass.Domain.Entities.Main;
using HiClass.Domain.Enums;

namespace HiClass.Domain.Entities.Notifications;

public class Notification
{
    public Guid NotificationId { get; set; }
    public Guid UserReceiverId { get; set; }
    public User? UserReceiver { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public NotificationType Type { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public NotificationStatus Status { get; set; }
    public string Message { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? ReadAt { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }
}