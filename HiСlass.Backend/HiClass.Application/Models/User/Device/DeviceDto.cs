using System.ComponentModel.DataAnnotations;
using static System.String;

namespace HiClass.Application.Models.User.Device;

public class DeviceDto
{
    [Required] public string DeviceToken { get; set; } = Empty;
}