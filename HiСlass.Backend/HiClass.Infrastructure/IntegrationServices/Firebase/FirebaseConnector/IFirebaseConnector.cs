using HiClass.Application.Models.Notifications;
using Microsoft.Extensions.Configuration;

namespace HiClass.Infrastructure.IntegrationServices.Firebase.FirebaseConnector;

public interface IFirebaseConnector
{
    Task<string> GetJwtTokenAsync();
}