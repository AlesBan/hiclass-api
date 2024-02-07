using Amazon.S3.Model;
using HiClass.Application.Models.AwsS3;
using Microsoft.AspNetCore.Http;

namespace HiClass.Infrastructure.Services.ImageServices;

public interface IUploadImageService
{
    Task<AwsS3UploadResponseDto> UploadImageAsync(IFormFile file, string folderTitle, int id);
}