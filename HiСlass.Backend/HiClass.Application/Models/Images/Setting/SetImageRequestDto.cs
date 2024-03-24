using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace HiClass.Application.Models.Images.Setting
{
    public class SetImageRequestDto
    {
        [Required] public IFormFile ImageFormFile { get; set; } = null!;
    }
}