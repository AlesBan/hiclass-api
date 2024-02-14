namespace HiClass.Application.Models.User;

public class ForgotPasswordResponseDto
{
    public string PasswordResetToken { get; set; }
    public string PasswordResetCode { get; set; }
}