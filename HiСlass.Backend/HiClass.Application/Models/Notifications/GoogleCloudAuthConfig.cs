using Microsoft.Extensions.Configuration;

namespace HiClass.Application.Models.Notifications;

public class GoogleCloudAuthConfig
{
    public string ProjectId { get; set; } = string.Empty;
    public string PrivateKeyId { get; set; } = string.Empty;
    public string PrivateKey { get; set; } = string.Empty;
    public string ClientEmail { get; set; } = string.Empty;
    public string ClientId { get; set; } = string.Empty;
    public string AuthUri { get; set; } = string.Empty;
    public string TokenUri { get; set; } = string.Empty;
    public string AuthProviderCertUrl { get; set; } = string.Empty;
    public string ClientCertUrl { get; set; } = string.Empty;

    public static GoogleCloudAuthConfig LoadFromEnv(IConfiguration configuration)
    {
        return new GoogleCloudAuthConfig
        {
            ProjectId = configuration["GOOGLE_PROJECT_ID"],
            PrivateKeyId = configuration["GOOGLE_PRIVATE_KEY_ID"],
            PrivateKey = configuration["GOOGLE_PRIVATE_KEY"].Replace("\\n", "\n"),
            ClientEmail = configuration["GOOGLE_CLIENT_EMAIL"],
            ClientId = configuration["GOOGLE_CLIENT_ID"],
            AuthUri = configuration["GOOGLE_AUTH_URI"],
            TokenUri = configuration["GOOGLE_TOKEN_URI"],
            AuthProviderCertUrl = configuration["GOOGLE_AUTH_PROVIDER_CERT_URL"],
            ClientCertUrl = configuration["GOOGLE_CLIENT_CERT_URL"]
        };
    }
}