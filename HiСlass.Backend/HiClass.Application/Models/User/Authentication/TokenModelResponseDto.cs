namespace HiClass.Application.Models.User.Authentication;

public class TokenModelResponseDto
{
    public string AccessToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
}