using Amazon.S3;
using Amazon.S3.Transfer;
using HiClass.Application.Models.AwsS3;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace HiClass.Infrastructure.Services.ImageServices;

public class ImageService : IImageService
{
    private readonly IConfiguration _configuration;

    public ImageService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<AwsS3UploadResponseDto> UploadImageAsync(IFormFile file, string folderTitle,
        int id)
    {
        var config = new AmazonS3Config()
        {
            RegionEndpoint = Amazon.RegionEndpoint.EUNorth1
        };
        
        var s3Object = await CreateAwsS3ObjectAsync(file, folderTitle, id);
        
        var response = new AwsS3UploadResponseDto();
        try
        {
            var uploadRequest = new TransferUtilityUploadRequest()
            {
                InputStream = s3Object.InputStream,
                Key = s3Object.FolderTitle + "/" + s3Object.Title,
                BucketName = s3Object.BucketTitle,
                CannedACL = S3CannedACL.NoACL,
            };

            var client = new AmazonS3Client(config);

            var transferUtility = new TransferUtility(client);

            await transferUtility.UploadAsync(uploadRequest);

            response.StatusCode = 200;
            response.Message = $"{s3Object.Title} was uploaded successfully";
        }
        catch (AmazonS3Exception exception)
        {
            response.StatusCode = (int)exception.StatusCode;
            response.Message = exception.Message;
        }
        catch (Exception exception)
        {
            response.StatusCode = 500;
            response.Message = exception.Message;
        }

        return response;
    }
    
    public async Task<AwsS3Object> CreateAwsS3ObjectAsync(IFormFile file, string folderTitle,
        int id)
    {
        await using var ms = new MemoryStream();
        await file.CopyToAsync(ms);

        var fileExtension = Path.GetExtension(file.FileName);
        var fileName = $"{id}.{fileExtension}";

        var bucketTitle = GetBucketTitleAsync();

        return new AwsS3Object()
        {
            InputStream = ms,
            Title = fileName,
            FolderTitle = folderTitle,
            BucketTitle = bucketTitle
        };
    }

    private string GetBucketTitleAsync()
    {
        var bucketTitle = _configuration["AWS_CONFIGURATION:BUCKETNAME"];
        return bucketTitle;
    }
}