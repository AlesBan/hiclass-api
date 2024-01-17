using HiClass.Application.Interfaces;
using MediatR;

namespace HiClass.Application.Handlers.EntityHandlers.InvitationHandlers.Commands.DeleteInvitation;

public class DeleteInvitationCommandHandler : IRequestHandler<DeleteInvitationCommand, Unit>
{
    private readonly ISharedLessonDbContext _context;
    public DeleteInvitationCommandHandler(ISharedLessonDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteInvitationCommand request, CancellationToken cancellationToken)
    {
        _context.Invitations.Remove(request.Invitation);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}