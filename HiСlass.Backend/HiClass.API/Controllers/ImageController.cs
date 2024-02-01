using System.Net;
using HiClass.API.Helpers;
using HiClass.Application.Models.AwsS3;
using HiClass.Infrastructure.Services.ImageServices;
using Microsoft.AspNetCore.Mvc;

namespace HiClass.API.Controllers;

public class ImageController : BaseController
{
    private readonly IImageService _imageService;

    public ImageController(IImageService imageService)
    {
        _imageService = imageService;
    }

    [HttpPost("user/{id:int}")]
    public async Task<IActionResult> Upload(IFormFile file, int id)
    {
        await using var ms = new MemoryStream();
        await file.CopyToAsync(ms);
        
        var fileExtension = Path.GetExtension(file.FileName);
        var fileName = $"{id}.{fileExtension}";
        
        var s3Object = new AwsS3Object()
        {
            InputStream = ms,
            Title = fileName,
            BucketTitle = "profile_images"
        };
        
        var result = await _imageService.UploadImageAsync(s3Object);

        return ResponseHelper.GetOkResult(result);
    }
}