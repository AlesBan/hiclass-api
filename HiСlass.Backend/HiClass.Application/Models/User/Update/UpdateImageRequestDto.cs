using Microsoft.AspNetCore.Http;

namespace HiClass.Application.Models.User.Update;

public class UpdateImageRequestDto
{
    public IFormFile ImageFormFile { get; set; }
}