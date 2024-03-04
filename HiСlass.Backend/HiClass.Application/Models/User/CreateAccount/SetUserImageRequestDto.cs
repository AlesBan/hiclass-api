using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace HiClass.Application.Models.User.CreateAccount;

public class SetUserImageRequestDto
{
    [Required] public IFormFile ImageFormFile{ get; set; }
}