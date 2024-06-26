using HiClass.Application.Models.Images.Aws;
using Microsoft.AspNetCore.Http;

namespace HiClass.Infrastructure.InternalServices.ImageServices;

public interface IImageHandlerService
{
    Task<ImageHandleResponseDto> UploadImageAsync(IFormFile file, string pathToStoreFile,
        string fileTitleToSave);
    Task<ImageHandleResponseDto> UpdateImageAsync(IFormFile file, string filePath,
        string fileTitle);
    
}