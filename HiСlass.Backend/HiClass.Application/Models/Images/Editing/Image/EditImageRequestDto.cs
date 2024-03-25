using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace HiClass.Application.Models.Images.Editing.Image;

public class EditImageRequestDto
{
    [Required] public IFormFile ImageFormFile { get; set; } = null!;
}