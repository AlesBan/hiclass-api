namespace HiClass.Application.Models.Images;

public class ImageHandleResponseDto
{
    public int StatusCode { get; set; } = 200;
    public string Message { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
}