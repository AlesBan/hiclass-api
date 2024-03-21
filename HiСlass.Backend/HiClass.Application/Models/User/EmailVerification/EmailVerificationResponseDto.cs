using static System.String;

namespace HiClass.Application.Models.User.EmailVerification;

public class EmailVerificationResponseDto
{
    public string AccessToken { get; set; } = Empty;
}