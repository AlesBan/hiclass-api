using HiClass.Domain.Entities.Main;
using HiClass.Domain.EntityConnections;

namespace HiClass.Domain.Entities.Notifications;

public class Device
{
    public Guid DeviceId { get; set; }
    public string DeviceToken { get; set; } = string.Empty;
    public ICollection<UserDevice> UserDevices { get; set; } = new List<UserDevice>();
}