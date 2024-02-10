using System.Net;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using HiClass.Application.Models.Images;
using HiClass.Application.Models.Images.Aws;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace HiClass.Infrastructure.Services.ImageServices.Aws;

public class AwsImagesService : IAwsImagesService
{
    private readonly IConfiguration _configuration;

    public AwsImagesService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<ImageHandleResponseDto> UploadImageAsync(IFormFile file, string pathToStoreFile,
        string fileTitleToSave)
    {
        var config = new AmazonS3Config()
        {
            RegionEndpoint = Amazon.RegionEndpoint.EUNorth1
        };

        using var ms = new MemoryStream();
        await file.CopyToAsync(ms);
        var s3Object = await CreateAwsS3ObjectAsync(file, pathToStoreFile, fileTitleToSave);

        var response = new ImageHandleResponseDto();
        try
        {
            var uploadRequest = new TransferUtilityUploadRequest()
            {
                InputStream = ms,
                Key = s3Object.PathToStoreFile + "/" + s3Object.Title,
                BucketName = s3Object.BucketTitle,
                CannedACL = S3CannedACL.NoACL,
            };

            var credentials = new BasicAWSCredentials(
                _configuration["AWS_CONFIGURATION:AWS_KEY"],
                _configuration["AWS_CONFIGURATION:AWS_SECRETKEY"]
            );

            var client = new AmazonS3Client(credentials, config);

            var transferUtility = new TransferUtility(client);

            await transferUtility.UploadAsync(uploadRequest);

            response.ImageUrl = $"https://s3.eu-north-1.amazonaws.com/{s3Object.BucketTitle}/{s3Object.PathToStoreFile}/{s3Object.Title}"; 
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

    public async Task<ImageHandleResponseDto> UpdateImageAsync(IFormFile file, string filePath, 
        string fileTitle)
    {
        var config = new AmazonS3Config()
        {
            RegionEndpoint = Amazon.RegionEndpoint.EUNorth1
        };
        var s3Object = await CreateAwsS3ObjectAsync(file, filePath, fileTitle);

        //delete
        var deleteRequest= new DeleteObjectRequest()
        {
            BucketName = s3Object.BucketTitle,
            Key = filePath
        };
        
        var credentials = new BasicAWSCredentials(
            _configuration["AWS_CONFIGURATION:AWS_KEY"],
            _configuration["AWS_CONFIGURATION:AWS_SECRETKEY"]
        );
        
        using var client = new AmazonS3Client(credentials, config);
        
        await client.DeleteObjectAsync(deleteRequest);
            
        
        //save new
        using var ms = new MemoryStream();
        await file.CopyToAsync(ms);

        var response = new ImageHandleResponseDto();
        try
        {
            var uploadRequest = new TransferUtilityUploadRequest()
            {
                InputStream = ms,
                Key = s3Object.PathToStoreFile + "/" + s3Object.Title,
                BucketName = s3Object.BucketTitle,
                CannedACL = S3CannedACL.NoACL,
            };

            var transferUtility = new TransferUtility(client);

            await transferUtility.UploadAsync(uploadRequest);

            response.ImageUrl = $"https://s3.eu-north-1.amazonaws.com/{s3Object.BucketTitle}/{s3Object.PathToStoreFile}/{s3Object.Title}"; 
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

    private async Task<AwsS3Object> CreateAwsS3ObjectAsync(IFormFile file, string pathToStoreFile,
        string fileTitle)
    {
        var fileExtension = Path.GetExtension(file.FileName);
        var fileTitleWithExtension = $"{fileTitle}{fileExtension}";

        var bucketTitle = GetBucketTitleAsync();

        return new AwsS3Object()
        {
            Title = fileTitleWithExtension,
            PathToStoreFile = pathToStoreFile,
            BucketTitle = bucketTitle
        };
    }

    private string GetBucketTitleAsync()
    {
        var bucketTitle = _configuration["AWS_CONFIGURATION:BUCKETNAME"];
        return bucketTitle;
    }
}