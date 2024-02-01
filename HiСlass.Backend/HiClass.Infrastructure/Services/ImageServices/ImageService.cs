using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using HiClass.Application.Models.AwsS3;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;

namespace HiClass.Infrastructure.Services.ImageServices;

public class ImageService : IImageService
{
    private readonly string _bucketName;
    private readonly string _awsKey;
    private readonly string _awsSecretKey;
    private readonly string _userImageFolder;
    private readonly string _classImageFolder;


    public ImageService(IConfiguration configuration)
    {
        _awsKey = configuration["AwsConfiguration:AwsKey"];
        _awsSecretKey = configuration["AwsConfiguration:AwsSecretKey"];
        _bucketName = configuration["AwsConfiguration:BucketName"];
        _userImageFolder = configuration["AwsConfiguration:UserImageFolder"];
        _classImageFolder = configuration["AwsConfiguration:ClassImageFolder"];
    }

    public async Task<AwsS3ResponseDto> UploadImageAsync(AwsS3Object s3Object)
    {
        var credentials = new BasicAWSCredentials(_awsKey, _awsSecretKey);
        var config = new AmazonS3Config()
        {
            RegionEndpoint = Amazon.RegionEndpoint.EUNorth1
        };

        var response = new AwsS3ResponseDto();
        try
        {
            var uploadRequest = new TransferUtilityUploadRequest()
            {
                InputStream = s3Object.InputStream,
                Key = s3Object.Title,
                BucketName = _bucketName,
                CannedACL = S3CannedACL.NoACL,
            };

            var client = new AmazonS3Client(credentials, config);

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
}