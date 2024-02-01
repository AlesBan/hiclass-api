using System.ComponentModel.DataAnnotations;

namespace HiClass.Application.Dtos.UserDtos.ResetPassword;

public class ResetPasswordRequestDto
{
    [Required] public string Password { get; set; } = string.Empty;
}