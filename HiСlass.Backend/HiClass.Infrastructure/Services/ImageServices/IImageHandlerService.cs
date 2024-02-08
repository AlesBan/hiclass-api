using HiClass.Application.Models.Images;
using Microsoft.AspNetCore.Http;

namespace HiClass.Infrastructure.Services.ImageServices;

public interface IImageHandlerService
{
    Task<UploadImageResponseDto> UploadImageAsync(IFormFile file, string pathToStoreFile,
        string fileTitle);

}