using System.ComponentModel.DataAnnotations;

namespace HiClass.Application.Models.User.Authentication;

public class GoogleSingInRequestDto
{
    [Required] public string Token { get; set; } = string.Empty;
    [Required] public string DeviceToken { get; set; } = string.Empty;
}