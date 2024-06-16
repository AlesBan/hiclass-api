using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace HiClass.API.Configuration;

public static class AuthenticationConfigurator
{
    public static void ConfigureAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = configuration["JWT_SETTINGS:VALID_ISSUER"],
                    ValidAudience = configuration["JWT_SETTINGS:VALID_AUDIENCE"],
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(configuration["JWT_SETTINGS:ISSUER_SIGNING_KEY"])),
                    ValidateIssuer = bool.Parse(configuration["JWT_SETTINGS:VALIDATE_ISSUER"]),
                    ValidateAudience = bool.Parse(configuration["JWT_SETTINGS:VALIDATE_AUDIENCE"]),
                    ValidateLifetime = bool.Parse(configuration["JWT_SETTINGS:VALIDATE_LIFETIME"]),
                    RequireExpirationTime = bool.Parse(configuration["JWT_SETTINGS:REQUIRE_EXPIRATION_TIME"]),
                    ValidateIssuerSigningKey = bool.Parse(configuration["JWT_SETTINGS:VALIDATE_ISSUER_SIGNING_KEY"]),
                };
            });
    }
}