using HiClass.Application.Models.AwsS3;
using Microsoft.AspNetCore.Http;

namespace HiClass.Application.Interfaces.Services;

public interface IUploadImageService
{
    Task<AwsS3UploadImageResponseDto> UploadImageAsync(IFormFile file, string folderTitle, Guid userId);
}