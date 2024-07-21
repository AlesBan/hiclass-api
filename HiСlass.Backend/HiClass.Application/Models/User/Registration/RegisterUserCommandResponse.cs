namespace HiClass.Application.Models.User.Registration;

public class RegisterUserCommandResponse
{
    public Guid UserId { get; set; }
    public string AccessToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public string VerificationCode { get; set; } = string.Empty;
}