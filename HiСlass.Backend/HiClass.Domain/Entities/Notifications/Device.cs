using HiClass.Domain.Entities.Main;
using HiClass.Domain.EntityConnections;

namespace HiClass.Domain.Entities.Notifications;

public class Device
{
    public Guid DeviceId { get; set; }
    public string DeviceToken { get; set; } = string.Empty;
    public Guid UserId { get; set; }
    public User User { get; set; }
    
}