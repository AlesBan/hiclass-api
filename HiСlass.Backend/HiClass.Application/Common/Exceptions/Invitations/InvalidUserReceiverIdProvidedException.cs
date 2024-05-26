using HiClass.Application.Interfaces.Exceptions;

namespace HiClass.Application.Common.Exceptions.Invitations;

public class InvalidUserReceiverIdProvidedException : Exception, IUiException
{
    public InvalidUserReceiverIdProvidedException(Guid invitationId, Guid userReceiverId) :
        base("The invitation with id " + invitationId + " does not have a receiver with id " + userReceiverId)
    {
    }
}