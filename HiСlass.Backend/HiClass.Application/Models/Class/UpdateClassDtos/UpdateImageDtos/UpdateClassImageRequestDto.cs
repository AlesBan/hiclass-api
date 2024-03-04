using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace HiClass.Application.Models.Class.UpdateClassDtos.UpdateImageDtos;

public class UpdateClassImageRequestDto
{
    [Required]
    public IFormFile ImageFormFile { get; set; }
}