using System.ComponentModel.DataAnnotations;

namespace HiClass.Application.Models.User.PasswordHandling;

public class ForgotPasswordResponseDto
{
    [Required] public string PasswordResetToken { get; set; }
    [Required] public string PasswordResetCode { get; set; }
}