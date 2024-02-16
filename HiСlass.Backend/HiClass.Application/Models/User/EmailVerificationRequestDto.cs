using System.ComponentModel.DataAnnotations;

namespace HiClass.Application.Models.User;

public class EmailVerificationRequestDto
{
    [Required] public string VerificationCode { get; set; } = null!;
}