using System.Text.Json.Serialization;
using HiClass.Domain.Enums;

namespace HiClass.Application.Models.Notifications;

public class UpdateNotificationStatusRequestDto
{
    public Guid NotificationId { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public NotificationStatus Status { get; set; }
}