namespace HiClass.Application.Models.Images.Aws;

public class AwsS3Object
{
    public string Title { get; set; } = null!;
    public string PathToStoreFile { get; set; } = null!;
    public MemoryStream InputStream { get; set; } = null!;
    public string BucketTitle { get; set; } = null!;
}