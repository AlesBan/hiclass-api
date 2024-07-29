using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;

namespace HiClass.API.Configuration
{
    public static class FirebaseConfigurator
    {
        public static void ConfigureFirebase(this IServiceCollection services, IConfiguration configuration)
        {
            FirebaseApp.Create(new AppOptions
            {
                Credential = GoogleCredential.FromFile(configuration["FIREBASE_CONFIGURATION:CONFIGURATION_FILEPATH"])
            });
        }
    }
}