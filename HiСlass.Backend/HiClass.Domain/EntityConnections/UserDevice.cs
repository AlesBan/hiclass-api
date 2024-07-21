using HiClass.Domain.Entities.Main;
using HiClass.Domain.Entities.Notifications;

namespace HiClass.Domain.EntityConnections;

public class UserDevice
{
    public Guid UserId { get; set; }
    public User? User { get; set; } 
    public Guid DeviceId { get; set; }
    public Device? Device { get; set; } 
    public string? RefreshToken { get; set; }
    public bool IsActive { get; set; }
    public DateTime LastActive { get; set; }
}