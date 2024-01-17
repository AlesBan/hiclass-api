using System.ComponentModel.DataAnnotations;
using HiClass.Domain.Entities.Main;
using Wonderworld.Application.Common.Mappings;

namespace HiClass.Application.Dtos.UserDtos.Authentication;

public class UserRegisterRequestDto : IMapWith<User>
{
    [Required, EmailAddress] public string Email { get; set; } = string.Empty;

    [Required] public string Password { get; set; } = string.Empty;
    
}