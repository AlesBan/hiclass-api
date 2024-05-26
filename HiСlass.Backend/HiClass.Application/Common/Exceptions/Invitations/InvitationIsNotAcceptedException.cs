using HiClass.Application.Interfaces.Exceptions;
using HiClass.Domain.Enums;

namespace HiClass.Application.Common.Exceptions.Invitations;

public class InvitationIsNotAcceptedException : Exception, IUiException
{
    public InvitationIsNotAcceptedException(InvitationStatus status) : base(
        $"Invitation is not accepted, status: {status}")
    {
    }
}