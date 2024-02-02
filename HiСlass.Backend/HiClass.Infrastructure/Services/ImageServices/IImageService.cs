using Amazon.S3.Model;
using HiClass.Application.Models.AwsS3;

namespace HiClass.Infrastructure.Services.ImageServices;

public interface IImageService
{
    Task<AwsS3ResponseDto> UploadImageAsync(AwsS3Object s3Object, AwsCredentials awsCredentials);
}