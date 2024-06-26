namespace HiClass.Application.Models.Device;

public class DeviceDto
{
    public Guid DeviceId { get; set; }
    public string DeviceToken { get; set; } = string.Empty;
}