using HiClass.Application.Models.Images;
using HiClass.Application.Models.Images.Aws;
using Microsoft.AspNetCore.Http;

namespace HiClass.Infrastructure.Services.ImageServices;

public interface IImageHandlerService
{
    Task<ImageHandleResponseDto> UploadImageAsync(IFormFile file, string pathToStoreFile,
        string fileTitleToSave);
    Task<ImageHandleResponseDto> UpdateImageAsync(IFormFile file, string filePath,
        string fileTitle);
    
}