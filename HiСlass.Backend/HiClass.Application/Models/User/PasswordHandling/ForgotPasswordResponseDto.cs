using System.ComponentModel.DataAnnotations;

namespace HiClass.Application.Models.User.PasswordHandling;

public class ForgotPasswordResponseDto
{
    [Required] public string PasswordResetToken { get; set; } = string.Empty;
}