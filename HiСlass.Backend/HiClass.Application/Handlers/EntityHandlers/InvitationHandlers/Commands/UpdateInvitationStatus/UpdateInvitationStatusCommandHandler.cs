using HiClass.Application.Interfaces;
using MediatR;

namespace HiClass.Application.Handlers.EntityHandlers.InvitationHandlers.Commands.UpdateInvitationStatus;

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
        var isAccepted = request.IsAccepted;

        var invitation = _context.Invitations.SingleOrDefault(i => i.InvitationId == invitationId);

        invitation.Status
    }
}