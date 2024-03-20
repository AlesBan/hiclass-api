using System.ComponentModel.DataAnnotations;

namespace HiClass.Application.Models.User.Authentication;

public class UserLoginRequestDto
{
    [Required, EmailAddress]
    public string Email { get; set; }
    
    [Required]
    public string Password { get; set; }
    
}