using System.ComponentModel.DataAnnotations;

namespace HiClass.Application.Models.User.Authentication;

public class LogOutRequestDto
{
    [Required] public string DeviceToken { get; set; } = string.Empty;
}