using HiClass.API.Helpers;
using HiClass.Application.Interfaces.Services;
using HiClass.Infrastructure.Services.ImageServices;
using HiClass.Infrastructure.Services.ImageServices.Aws;
using Microsoft.AspNetCore.Mvc;

namespace HiClass.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ImageController : ControllerBase
    {
        private readonly IImageHandlerService _imageService;
        private readonly IConfiguration _configuration;
        

        public ImageController(IImageHandlerService imageService, IConfiguration configuration)
        {
            _imageService = imageService;
            _configuration = configuration;
        }

        [HttpPost("user/{id:guid}")]
        public async Task<IActionResult> UploadUserImage(IFormFile file, Guid id)
        {
            var pathToStore = _configuration["AWS_CONFIGURATION:USER_IMAGES_FOLDER"];

            var result = await _imageService.UploadImageAsync(file, pathToStore, id.ToString());

            return ResponseHelper.GetOkResult(result);
        }

        [HttpPost("class/{id:guid}")]
        public async Task<IActionResult> UploadClassImage(IFormFile file, Guid id)
        {
            var pathToStore = _configuration["AWS_CONFIGURATION:CLASS_IMAGES_FOLDER"];

            var result = await _imageService.UploadImageAsync(file, pathToStore, id.ToString());

            return ResponseHelper.GetOkResult(result);
        }
    }
}