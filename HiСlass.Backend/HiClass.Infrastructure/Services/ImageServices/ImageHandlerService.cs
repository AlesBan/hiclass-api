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

    public Task<ImageHandleResponseDto> UploadImageAsync(IFormFile file, string pathToStoreFile, string fileTitleToSave)
    {
        var result = _imageService.UploadImageAsync(file, pathToStoreFile, fileTitleToSave);

        return result;
    }

    public Task<ImageHandleResponseDto> UpdateImageAsync(IFormFile file, string filePath, string fileTitle)
    {
        return _imageService.UpdateImageAsync(file, filePath, fileTitle);
    }
}