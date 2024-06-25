using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using HiClass.Application.Common.Exceptions.Firebase;
using HiClass.Application.Models.Notifications;
using Microsoft.Extensions.Configuration;

namespace HiClass.Infrastructure.IntegrationServices.Firebase.FireBaseNotificationSender;

public class FireBaseNotificationSender : IFireBaseNotificationSender
{
    
    private readonly IConfiguration _configuration;
    public FireBaseNotificationSender(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public async Task SendNotificationAsync(NotificationResponseDto notificationDto, List<string> deviceTokens)
    {
        var jsonPath = _configuration["FIREBASE_CONFIGURATION:CONFIGURATION_FILEPATH"];

        GoogleCredential googleCredential;
        await using (var stream = new FileStream(jsonPath, FileMode.Open, FileAccess.Read))
        {
            FirebaseApp.Create(new AppOptions()
            {
                Credential  = await GoogleCredential.FromStreamAsync(stream, CancellationToken.None)
            });
        }

        var message = new MulticastMessage()
        {
            Data = new Dictionary<string, string>()
            {
                { "NotificationType", $"{notificationDto.NotificationType}" },
                { "Sender", $"{notificationDto.NotificationMessage.Sender}" },
                { "Message", $"{notificationDto.NotificationMessage.Message}" },
            },
            Tokens = deviceTokens,
            Notification = new Notification()
            {
                Title = "HiClassAPI Notification",
                Body = $"{notificationDto.NotificationMessage.Message}"
            }
        };

        try
        {
            var response = await FirebaseMessaging.DefaultInstance.SendEachForMulticastAsync(message);
        }
        catch
        {
            throw new FirebaseMessagingDeliveryException();
        }
    }
}