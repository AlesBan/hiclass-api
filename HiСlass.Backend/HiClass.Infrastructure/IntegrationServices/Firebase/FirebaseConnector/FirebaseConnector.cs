using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using HiClass.Application.Common.Exceptions.Firebase;
using HiClass.Application.Models.Notifications;
using Microsoft.Extensions.Configuration;

namespace HiClass.Infrastructure.IntegrationServices.Firebase.FirebaseConnector;

public class FirebaseConnector : IFirebaseConnector
{
    private readonly IConfiguration _configuration;
    private static readonly string[] Scopes = { "https://www.googleapis.com/auth/firebase.messaging" };

    public FirebaseConnector(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<string> GetJwtTokenAsync()
    {
        var jsonPath = _configuration["FIREBASE_CONFIGURATION:CONFIGURATION_FILEPATH"];

        GoogleCredential googleCredential;
        await using (var stream = new FileStream(jsonPath, FileMode.Open, FileAccess.Read))
        {
            googleCredential = await GoogleCredential.FromStreamAsync(stream, CancellationToken.None);
        }

        if (googleCredential.IsCreateScopedRequired)
        {
            googleCredential = googleCredential.CreateScoped(Scopes);
        }

        var accessToken = await googleCredential.UnderlyingCredential.GetAccessTokenForRequestAsync();
        return accessToken;
    }

    
}