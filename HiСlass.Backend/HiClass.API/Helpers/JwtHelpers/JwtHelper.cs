using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using HiClass.Application.Common.Exceptions.Authentication;
using HiClass.Application.Dtos.UserDtos;
using HiClass.Application.Models.User;
using HiClass.Domain.Entities.Main;
using static System.Boolean;

namespace HiClass.API.Helpers.JwtHelpers;

public static class JwtHelper
{
    public static Guid GetUserIdFromClaims(HttpContext httpContext)
    {
        var decodedToken = GetTokenFromHeader(httpContext);

        var nameIdentifier = decodedToken.Claims
            .FirstOrDefault(claim =>
                claim.Type == "nameid")?
            .Value;

        if (nameIdentifier == null)
        {
            throw new InvalidTokenProvidedException();
        }

        return Guid.Parse(nameIdentifier);
    }

    public static bool GetIsCreatedAccountFromClaims(HttpContext httpContext)
    {
        var decodedToken = GetTokenFromHeader(httpContext);

        try
        {
            TryParse(decodedToken.Claims
                .FirstOrDefault(claim =>
                    claim.Type == "isCreatedAccount")?
                .Value, out var isCreatedAccount);

            return isCreatedAccount;
        }
        catch
        {
            throw new InvalidTokenProvidedException();
        }
    }
    
    public static bool GetIsVerifiedFromClaims(HttpContext httpContext)
    {
        var decodedToken = GetTokenFromHeader(httpContext);

        try
        {
            TryParse(decodedToken.Claims
                .FirstOrDefault(claim =>
                    claim.Type == "isVerified")?
                .Value, out var isVerified);

            return isVerified;
        }
        catch
        {
            throw new InvalidTokenProvidedException();
        }
    }

    public static PositionInfoDto GetPositionInfoFromClaims(HttpContext httpContext)
    {
        var decodedToken = GetTokenFromHeader(httpContext);

        try
        {
            TryParse(decodedToken.Claims
                .FirstOrDefault(claim =>
                    claim.Type == "isATeacher")?
                .Value, out var isATeacher);
            TryParse(decodedToken.Claims
                .FirstOrDefault(claim =>
                    claim.Type == "isATeacher")?
                .Value, out var isAnExpert);
            
            var positionInfo = new PositionInfoDto
            {
                IsTeacher = isATeacher,
                IsAnExpert = isAnExpert
            };
            return positionInfo;
        }
        catch
        {
            throw new InvalidTokenProvidedException();
        }
    }

    private static JwtSecurityToken GetTokenFromHeader(HttpContext httpContext)
    {
        try
        {
            var jwtToken = httpContext.Request
                .Headers["Authorization"]
                .ToString();
            var jwtHandler = new JwtSecurityTokenHandler();
            var decodedToken = jwtHandler.ReadJwtToken(jwtToken.Split(' ')[1]);

            return decodedToken;
        }
        catch
        {
            throw new InvalidTokenProvidedException();
        }
    }

    private static void SetRoleClaims(this ICollection<Claim> claims, User user)
    {
        user.UserRoles.Select(ur =>
                ur.Role.Title)
            .ToList()
            .ForEach(role =>
                claims.Add(new Claim(ClaimTypes.Role, role)));
    }
}