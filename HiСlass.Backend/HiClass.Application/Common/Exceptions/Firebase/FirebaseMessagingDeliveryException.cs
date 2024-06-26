using HiClass.Application.Interfaces.Exceptions;

namespace HiClass.Application.Common.Exceptions.Firebase;

public class FirebaseMessagingDeliveryException : Exception, IServerException
{
    public FirebaseMessagingDeliveryException() : base("Firebase messaging delivery exception")
    {
    }
}