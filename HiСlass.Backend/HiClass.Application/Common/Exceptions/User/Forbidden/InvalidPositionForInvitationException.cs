using HiClass.Application.Interfaces.Exceptions;

namespace HiClass.Application.Common.Exceptions.User.Forbidden;

public class InvalidPositionForSendingInvitationException : Exception, IUiException
{
    public InvalidPositionForSendingInvitationException() : base("Expert unable to send invitation."){
    }
}