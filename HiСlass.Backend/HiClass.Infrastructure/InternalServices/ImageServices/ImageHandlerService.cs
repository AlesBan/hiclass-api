using HiClass.Application.Models.Images.Aws;
using HiClass.Infrastructure.IntegrationServices.Aws;
using Microsoft.AspNetCore.Http;

namespace HiClass.Infrastructure.InternalServices.ImageServices;

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