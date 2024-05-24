namespace HiClass.Application.Common.Exceptions.Invitations;

public class InvalidUserReceiverIdProvidedException : Exception
{
    public InvalidUserReceiverIdProvidedException(Guid invitationId, Guid userReceiverId) :
        base("The invitation with id " + invitationId + " does not have a receiver with id " + userReceiverId)
    {
    }
}