using System.ComponentModel.DataAnnotations;

namespace HiClass.Application.Models.User.Authentication;

public class RefreshTokenRequestDto
{
    [Required] public string RefreshToken { get; set; } = string.Empty;
    [Required] public string DeviceToken { get; set; } = string.Empty;
}