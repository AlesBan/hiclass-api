using System.ComponentModel.DataAnnotations;

namespace HiClass.Application.Models.User.RevokeToken;

public class RevokeTokenRequestDto
{
    [Required] public string DeviceToken { get; set; } = string.Empty;
}