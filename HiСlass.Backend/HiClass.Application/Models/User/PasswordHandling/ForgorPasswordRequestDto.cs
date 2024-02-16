using System.ComponentModel.DataAnnotations;

namespace HiClass.Application.Models.User.PasswordHandling;

public class ForgotPasswordRequestDto
{
    [Required] public string Email { get; set; } = null!;
}