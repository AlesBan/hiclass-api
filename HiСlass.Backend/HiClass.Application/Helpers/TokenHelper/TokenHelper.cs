using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using HiClass.Application.Common.Exceptions.Configuration;
using HiClass.Application.Models.User.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace HiClass.Application.Helpers.TokenHelper;

public class TokenHelper : ITokenHelper
{
    private readonly IConfiguration _configuration;
    private readonly SymmetricSecurityKey _signingKey;
    private readonly string _validIssuer;
    private readonly string _validAudience;

    public TokenHelper(IConfiguration configuration)
    {
        _configuration = configuration;
        _signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(GetConfigurationValue("JWT_SETTINGS:ISSUER_SIGNING_KEY")));
        _validIssuer = GetConfigurationValue("JWT_SETTINGS:VALID_ISSUER");
        _validAudience = GetConfigurationValue("JWT_SETTINGS:VALID_AUDIENCE");
    }

    public string CreateAccessToken(CreateTokenDto userDto)
    {
        var jwtClaims = GetClaims(userDto);
        var expiresTime = DateTime.UtcNow.AddMinutes(GetConfigurationValueAsInt("AUTH_CONSTANTS:ACCESS_TOKEN_VALIDITY_MINUTES"));
        return GenerateToken(jwtClaims, expiresTime);
    }

    public string CreateRefreshToken(CreateTokenDto userDto)
    {
        var jwtClaims = GetClaims(userDto);
        var expiresTime = DateTime.UtcNow.AddDays(GetConfigurationValueAsInt("AUTH_CONSTANTS:REFRESH_TOKEN_VALIDITY_DAYS"));
        return GenerateToken(jwtClaims, expiresTime);
    }

    public bool ValidateToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        try
        {
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = _signingKey,
                ValidateIssuer = true,
                ValidIssuer = _validIssuer,
                ValidateAudience = true,
                ValidAudience = _validAudience,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            return validatedToken != null;
        }
        catch
        {
            return false;
        }
    }

    private string GetConfigurationValue(string key)
    {
        var value = _configuration[key];
        if (string.IsNullOrEmpty(value))
        {
            throw new InvalidConfigurationValueException(key);
        }

        return value;
    }

    private int GetConfigurationValueAsInt(string key)
    {
        var valueString = GetConfigurationValue(key);
        if (!int.TryParse(valueString, out var value))
        {
            throw new InvalidConfigurationValueException(key);
        }

        return value;
    }

    private string GenerateToken(IEnumerable<Claim> jwtClaims, DateTime expiresTime)
    {
        var credentials = new SigningCredentials(_signingKey, SecurityAlgorithms.HmacSha256);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(jwtClaims),
            Expires = expiresTime,
            Issuer = _validIssuer,
            Audience = _validAudience,
            SigningCredentials = credentials
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    private static IEnumerable<Claim> GetClaims(CreateTokenDto userDto)
    {
        return new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, userDto.UserId.ToString()),
            new(ClaimTypes.Email, userDto.Email),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64),
            new("isVerified", userDto.IsVerified.ToString()),
            new("isCreatedAccount", userDto.IsCreatedAccount.ToString()),
            new("isATeacher", userDto.IsATeacher.ToString()),
            new("isAExpert", userDto.IsAnExpert.ToString())
        };
    }
}
