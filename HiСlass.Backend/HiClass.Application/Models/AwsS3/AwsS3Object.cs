namespace HiClass.Application.Models.AwsS3;

public class AwsS3Object
{
   public string Title { get; set; } = null!;
   public MemoryStream InputStream { get; set; }= null!;
   public string BucketTitle { get; set; } = null!;
}