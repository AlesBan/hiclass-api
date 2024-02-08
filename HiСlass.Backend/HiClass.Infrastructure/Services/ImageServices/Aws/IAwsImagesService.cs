using HiClass.Application.Models.Images;
using Microsoft.AspNetCore.Http;

namespace HiClass.Infrastructure.Services.ImageServices.Aws;

public interface IAwsImagesService
{
    Task<UploadImageResponseDto> UploadImageAsync(IFormFile file, string pathToStoreFile,
        string fileTitle);
}