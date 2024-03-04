using System.ComponentModel.DataAnnotations;

namespace HiClass.Application.Models.User.CreateAccount;

public class SetUserImageResponseDto
{
    [Required] public string ImageUrl { get; set; } = string.Empty;
}