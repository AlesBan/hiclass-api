using HiClass.API.Helpers;
using HiClass.Application.Interfaces.Services;
using HiClass.Infrastructure.Services.ImageServices;
using Microsoft.AspNetCore.Mvc;

namespace HiClass.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ImageController : ControllerBase
    {
        private readonly IUploadImageService _imageService;
        private readonly IConfiguration _configuration;
        

        public ImageController(IUploadImageService imageService, IConfiguration configuration)
        {
            _imageService = imageService;
            _configuration = configuration;
        }

        [HttpPost("user/{id:guid}")]
        public async Task<IActionResult> UploadUserImage(IFormFile file, Guid id)
        {
            var folderTitle = _configuration["AWS_CONFIGURATION:USER_IMAGES_FOLDER"];

            var result = await _imageService.UploadImageAsync(file, folderTitle, id);

            return ResponseHelper.GetOkResult(result);
        }

        [HttpPost("class/{id:guid}")]
        public async Task<IActionResult> UploadClassImage(IFormFile file, Guid id)
        {
            var folderTitle = _configuration["AWS_CONFIGURATION:CLASS_IMAGES_FOLDER"];

            var result = await _imageService.UploadImageAsync(file, folderTitle, id);

            return ResponseHelper.GetOkResult(result);
        }
    }
}