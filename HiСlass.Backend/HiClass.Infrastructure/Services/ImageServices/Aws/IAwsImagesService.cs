using HiClass.Application.Models.Images;
using Microsoft.AspNetCore.Http;

namespace HiClass.Infrastructure.Services.ImageServices.Aws;

public interface IAwsImagesService
{
    Task<ImageHandleResponseDto> UploadImageAsync(IFormFile file, string pathToStoreFile,
        string fileTitleToSave);
    Task<ImageHandleResponseDto> UpdateImageAsync(IFormFile file, string filePath,
        string fileTitle);
}