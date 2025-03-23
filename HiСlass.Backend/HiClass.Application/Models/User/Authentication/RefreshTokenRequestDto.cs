using System.ComponentModel.DataAnnotations;

namespace HiClass.Application.Models.User.Authentication;

public class RefreshTokenRequestDto
{
    public string RefreshToken { get; set; } = string.Empty;
    public string DeviceToken { get; set; } = string.Empty;
}