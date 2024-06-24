using System.ComponentModel.DataAnnotations;

namespace HiClass.Application.Models.User.Authentication;

public class UserLoginRequestDto
{
    [Required, EmailAddress] public string Email { get; set; } = string.Empty;

    [Required] public string Password { get; set; } = string.Empty;
}