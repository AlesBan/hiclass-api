using System.ComponentModel.DataAnnotations;

namespace HiClass.Application.Models.User.RevokeToken;

public class RevokeTokenRequestDto
{
    public string DeviceToken { get; set; } = string.Empty;
    [Required] public string RefreshToken { get; set; } = string.Empty;
}