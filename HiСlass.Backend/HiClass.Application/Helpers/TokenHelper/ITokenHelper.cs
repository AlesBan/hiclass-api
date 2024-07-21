using HiClass.Application.Models.User.Authentication;

namespace HiClass.Application.Helpers.TokenHelper;

public interface ITokenHelper
{
    public string CreateAccessToken(CreateTokenDto userDto);
    public string CreateRefreshToken(CreateTokenDto userDto);
    public bool ValidateToken(string token);
}