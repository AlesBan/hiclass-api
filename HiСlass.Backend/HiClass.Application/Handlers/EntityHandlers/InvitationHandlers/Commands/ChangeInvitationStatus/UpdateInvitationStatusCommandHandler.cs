using HiClass.Application.Common.Exceptions.Database;
using HiClass.Application.Common.Exceptions.Invitations;
using HiClass.Application.Interfaces;
using HiClass.Domain.Entities.Communication;
using HiClass.Domain.Enums;
using MediatR;

namespace HiClass.Application.Handlers.EntityHandlers.InvitationHandlers.Commands.ChangeInvitationStatus;

public class UpdateInvitationStatusCommandHandler : IRequestHandler<UpdateInvitationStatusCommand>
{
    private readonly ISharedLessonDbContext _context;

    public UpdateInvitationStatusCommandHandler(ISharedLessonDbContext context)
    {
        _context = context;
    }

    public Task<Unit> Handle(UpdateInvitationStatusCommand request, CancellationToken cancellationToken)
    {
        var invitationId = request.InvitationId;

        var invitation = _context.Invitations.SingleOrDefault(i => i.InvitationId == invitationId);

        if (invitation == null)
        {
            throw new NotFoundException(nameof(Invitation), invitationId);
        }

        if (invitation.UserReceiverId != request.UserId)
        {
            throw new InvalidUserReceiverIdProvidedException(invitation.InvitationId, request.UserId, invitation.UserReceiverId);
        }

        invitation.Status = request.Status;

        _context.Invitations.Update(invitation);
        
        _context.SaveChangesAsync(cancellationToken);

        return Task.FromResult(Unit.Value);
    }
}