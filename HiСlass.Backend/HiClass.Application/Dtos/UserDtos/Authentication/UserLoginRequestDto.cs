using System.ComponentModel.DataAnnotations;

namespace HiClass.Application.Dtos.UserDtos.Authentication;

public class UserLoginRequestDto
{
    [Required, EmailAddress]
    public string Email { get; set; }
    
    [Required]
    public string Password { get; set; }
    
}