using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace HiClass.Application.Models.Class.SetImageDtos
{
    public class SetImageRequestDto
    {
        [Required]
        public IFormFile ImageFormFile { get; set; }
    }
}