using System.ComponentModel.DataAnnotations;

namespace HiClass.Application.Models.User.PasswordHandling;

public class ResetPasswordRequestDto
{
    [Required] public string Password { get; set; } = string.Empty;
}