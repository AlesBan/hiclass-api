using System.ComponentModel.DataAnnotations;

namespace HiClass.Application.Models.User.PasswordHandling;

public class CheckResetPasswordCodeDto
{
    [Required] public string Email { get; set; } = null!;
    [Required] public string ResetCode { get; set; } = null!;
}