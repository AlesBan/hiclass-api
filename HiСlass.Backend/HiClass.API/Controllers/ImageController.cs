using HiClass.API.Helpers;
using HiClass.Application.Models.AwsS3;
using HiClass.Infrastructure.Services.ImageServices;
using Microsoft.AspNetCore.Mvc;

namespace HiClass.API.Controllers;

public class ImageController : BaseController
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
        await using var ms = new MemoryStream();
        await file.CopyToAsync(ms);

        var fileExtension = Path.GetExtension(file.FileName);
        var fileName = $"{id}.{fileExtension}";

        var s3Object = new AwsS3Object()
        {
            InputStream = ms,
            Title = fileName,
            FolderTitle = _configuration["AWS_CONFIGURATION:USER_IMAGES_FOLDER"],
            BucketTitle = _configuration["AWS_CONFIGURATION:BUCKETNAME"],
        };

        var awsCredentials = new AwsCredentials()
        {
            AwsKey = _configuration["AWS_CONFIGURATION:AWS_KEY"],
            AwsSecretKey = _configuration["AWS_CONFIGURATION:AWS_SECRETKEY"]
        };

        var result = await _imageService.UploadImageAsync(s3Object, awsCredentials);

        return ResponseHelper.GetOkResult(result);
    }

    [HttpPost("class/{id:int}")]
    public async Task<IActionResult> UploadClassImage(IFormFile file, int id)
    {
        await using var ms = new MemoryStream();
        await file.CopyToAsync(ms);

        var fileExtension = Path.GetExtension(file.FileName);
        var fileName = $"{id}.{fileExtension}";

        var s3Object = new AwsS3Object()
        {
            InputStream = ms,
            Title = fileName,
            FolderTitle = _configuration["AWS_CONFIGURATION:CLASS_IMAGES_FOLDER"],
            BucketTitle = _configuration["AWS_CONFIGURATION:BUCKETNAME"]
        };

        var awsCredentials = new AwsCredentials()
        {
            AwsKey = _configuration["AWS_CONFIGURATION:AWS_KEY"],
            AwsSecretKey = _configuration["AWS_CONFIGURATION:AWS_SECRETKEY"]
        };
        var result = await _imageService.UploadImageAsync(s3Object, awsCredentials);

        return ResponseHelper.GetOkResult(result);
    }
}