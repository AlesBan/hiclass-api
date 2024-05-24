using HiClass.Application.Common.Exceptions.Database;
using HiClass.Application.Common.Exceptions.Invitations;
using HiClass.Application.Interfaces;
using HiClass.Domain.Entities.Communication;
using HiClass.Domain.Enums;
using MediatR;

namespace HiClass.Application.Handlers.EntityHandlers.InvitationHandlers.Commands.ChangeInvitationStatus;

public class ChangeInvitationStatusCommandHandler : IRequestHandler<ChangeInvitationStatusCommand>
{
    private readonly ISharedLessonDbContext _context;

    public ChangeInvitationStatusCommandHandler(ISharedLessonDbContext context)
    {
        _context = context;
    }

    public Task<Unit> Handle(ChangeInvitationStatusCommand request, CancellationToken cancellationToken)
    {
        var invitationId = request.InvitationId;
        var isAccepted = request.IsAccepted;

        var invitation = _context.Invitations.SingleOrDefault(i => i.InvitationId == invitationId);

        if (invitation == null)
        {
            throw new NotFoundException(nameof(Invitation), invitationId);
        }

        if (invitation.UserReceiverId != request.UserReceiverId)
        {
            throw new InvalidUserReceiverIdProvidedException(invitation.InvitationId, request.UserReceiverId);
        }

        invitation.Status = isAccepted ? InvitationStatus.Accepted.ToString() : InvitationStatus.Declined.ToString();

        _context.Invitations.Update(invitation);

        return Task.FromResult(Unit.Value);
    }
}