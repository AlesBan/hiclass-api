using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Transfer;
using HiClass.Application.Models.AwsS3;

namespace HiClass.Infrastructure.Services.ImageServices;

public class ImageService : IImageService
{
    public async Task<AwsS3ResponseDto> UploadImageAsync(AwsS3Object s3Object, AwsCredentials awsCredentials)
    {
        var credentials = new BasicAWSCredentials(awsCredentials.AwsKey, awsCredentials.AwsSecretKey);
        
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
                Key = s3Object.FolderTitle + "/" + s3Object.Title,
                BucketName = s3Object.BucketTitle,
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