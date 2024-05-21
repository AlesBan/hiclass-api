using System.ComponentModel.DataAnnotations;

namespace HiClass.Application.Models.User.EmailVerification;

public class EmailVerificationRequestDto
{
    [Required] public string Email { get; set; } = string.Empty;
    [Required] public string VerificationCode { get; set; } = null!;
}