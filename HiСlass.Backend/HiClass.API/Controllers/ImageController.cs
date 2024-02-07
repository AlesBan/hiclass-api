using HiClass.API.Helpers;
using HiClass.Application.Models.AwsS3;
using HiClass.Infrastructure.Services.ImageServices;
using Microsoft.AspNetCore.Mvc;

namespace HiClass.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ImageController : ControllerBase
    {
        private readonly IImageService _imageService;
        private readonly IConfiguration _configuration;

        public ImageController(IImageService imageService, IConfiguration configuration)
        {
            _imageService = imageService;
            _configuration = configuration;
        }

        [HttpPost("user/{id:int}")]
        public async Task<IActionResult> UploadUserImage(IFormFile file, int id)
        {
            var folderTitle = _configuration["AWS_CONFIGURATION:USER_IMAGES_FOLDER"];

            var result = await _imageService.UploadImageAsync(file, folderTitle, id);

            return ResponseHelper.GetOkResult(result);
        }

        [HttpPost("class/{id:int}")]
        public async Task<IActionResult> UploadClassImage(IFormFile file, int id)
        {
            var folderTitle = _configuration["AWS_CONFIGURATION:CLASS_IMAGES_FOLDER"];

            var result = await _imageService.UploadImageAsync(file, folderTitle, id);

            return ResponseHelper.GetOkResult(result);
        }
    }
}