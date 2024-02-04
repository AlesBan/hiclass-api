namespace HiClass.Application.Models.AwsS3;

public class AwsS3UploadResponseDto
{
    public int StatusCode { get; set; } = 200;
    public string Message { get; set; } = string.Empty;
}