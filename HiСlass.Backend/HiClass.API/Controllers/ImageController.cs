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

            var s3Object = await CreateAwsS3ObjectAsync(file, folderTitle, id);
            var result = await _imageService.UploadImageAsync(s3Object);

            return ResponseHelper.GetOkResult(result);
        }

        [HttpPost("class/{id:int}")]
        public async Task<IActionResult> UploadClassImage(IFormFile file, int id)
        {
            var folderTitle = _configuration["AWS_CONFIGURATION:CLASS_IMAGES_FOLDER"];

            var s3Object = await CreateAwsS3ObjectAsync(file, folderTitle, id);
            var result = await _imageService.UploadImageAsync(s3Object);

            return ResponseHelper.GetOkResult(result);
        }

        private async Task<AwsS3Object> CreateAwsS3ObjectAsync(IFormFile file, string folderTitle,
            int id)
        {
            await using var ms = new MemoryStream();
            await file.CopyToAsync(ms);

            var fileExtension = Path.GetExtension(file.FileName);
            var fileName = $"{id}.{fileExtension}";
            
            var bucketTitle = GetBucketTitleAsync();
            
            return new AwsS3Object()
            {
                InputStream = ms,
                Title = fileName,
                FolderTitle = folderTitle,
                BucketTitle = bucketTitle
            };
        }

        private string GetBucketTitleAsync()
        {
            var bucketTitle = _configuration["AWS_CONFIGURATION:BUCKETNAME"];
            return bucketTitle;
        }
    }
}