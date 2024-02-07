namespace HiClass.Application.Models.AwsS3;

public class AwsS3UploadImageResponseDto
{
    public int StatusCode { get; set; } = 200;
    public string Message { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
}