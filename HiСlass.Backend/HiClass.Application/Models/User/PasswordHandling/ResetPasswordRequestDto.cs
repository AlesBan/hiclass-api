using System.ComponentModel.DataAnnotations;

namespace HiClass.Application.Models.User.PasswordHandling;

public class ResetPasswordRequestDto
{
    public string DeviceToken { get; set; } = string.Empty;
    [Required] public string NewPassword { get; set; } = string.Empty;
    [Required] public string RefreshToken { get; set; } = string.Empty;
}