using HiClass.Application.Models.Images;
using HiClass.Application.Models.Images.Aws;
using HiClass.Infrastructure.Services.ImageServices.Aws;
using Microsoft.AspNetCore.Http;

namespace HiClass.Infrastructure.Services.ImageServices;

public class ImageHandlerService : IImageHandlerService
{
    private readonly IAwsImagesService _awsImageService;

    public ImageHandlerService(IAwsImagesService imageService)
    {
        _awsImageService = imageService;
    }

    public Task<ImageHandleResponseDto> UploadImageAsync(IFormFile file, string pathToStoreFile, string fileTitleToSave)
    {
        var result = _awsImageService.UploadImageAsync(file, pathToStoreFile, fileTitleToSave);

        return result;
    }

    public Task<ImageHandleResponseDto> UpdateImageAsync(IFormFile file, string filePath, string fileTitle)
    {
        return _awsImageService.UpdateImageAsync(file, filePath, fileTitle);
    }
}