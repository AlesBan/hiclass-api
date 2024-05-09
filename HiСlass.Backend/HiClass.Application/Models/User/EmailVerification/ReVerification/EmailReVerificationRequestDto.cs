using System.ComponentModel.DataAnnotations;

namespace HiClass.Application.Models.User.EmailVerification.ReVerification;

public class EmailReVerificationRequestDto
{
    [Required] public string Email { get; set; } = string.Empty;
}