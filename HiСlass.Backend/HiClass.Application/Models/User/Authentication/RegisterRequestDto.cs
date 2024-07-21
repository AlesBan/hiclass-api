using System.ComponentModel.DataAnnotations;
using HiClass.Application.Common.Mappings;

namespace HiClass.Application.Models.User.Authentication;

public class RegisterRequestDto : IMapWith<Domain.Entities.Main.User>
{
    [Required, EmailAddress] public string Email { get; set; } = string.Empty;
    [Required] public string Password { get; set; } = string.Empty;
    [Required] public string DeviceToken { get; set; } = string.Empty;
}