using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace HiClass.Application.Models.Images.Editing.Banner;

public class EditBannerImageRequestDto
{
    [Required] public IFormFile ImageFormFile { get; set; } = null!;

}