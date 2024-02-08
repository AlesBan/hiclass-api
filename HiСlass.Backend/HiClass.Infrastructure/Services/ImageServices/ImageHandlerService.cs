using HiClass.Application.Models.Images;
using HiClass.Infrastructure.Services.ImageServices.Aws;
using Microsoft.AspNetCore.Http;

namespace HiClass.Infrastructure.Services.ImageServices;

public class ImageHandlerService : IImageHandlerService
{
    private readonly IAwsImagesService _imageService;

    public ImageHandlerService(IAwsImagesService imageService)
    {
        _imageService = imageService;
    }

    public Task<UploadImageResponseDto> UploadImageAsync(IFormFile file, string pathToStoreFile, string fileTitle)
    {
        var result = _imageService.UploadImageAsync(file, pathToStoreFile, fileTitle);

        return result;
    }
}