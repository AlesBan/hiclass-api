using HiClass.Application.Common.Exceptions.Class;
using HiClass.Application.Interfaces;
using HiClass.Domain.Entities.Communication;
using MediatR;

namespace HiClass.Application.Handlers.EntityHandlers.InvitationHandlers.Commands.CreateInvitation;

public class CreateInvitationCommandHandler : IRequestHandler<CreateInvitationCommand, Invitation>
{
    private readonly ISharedLessonDbContext _context;

    public CreateInvitationCommandHandler(ISharedLessonDbContext context)
    {
        _context = context;
    }

    public async Task<Invitation> Handle(CreateInvitationCommand request, CancellationToken cancellationToken)
    {
        var invitation = new Invitation
        {
            UserSenderId = request.UserSenderId,
            UserReceiverId = request.UserReceiverId,
            ClassSenderId = request.ClassSenderId,
            ClassReceiverId = request.ClassReceiverId,
            DateOfInvitation = request.DateOfInvitation.ToUniversalTime(),
            InvitationText = request.InvitationText,
            Status = request.Status,
            CreatedAt = DateTime.UtcNow
        };

        ValidateClassReceiverId(request.UserSenderId, request.ClassReceiverId);
        
        await _context.Invitations.AddAsync(invitation, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return invitation;
    }

    private void ValidateClassReceiverId(Guid userSenderId, Guid classReceiverId)
    {
        var classReceiverIsUserSenderClass = _context.Classes
            .Any(c => c.ClassId == classReceiverId && c.UserId == userSenderId);

        if (classReceiverIsUserSenderClass)
        {
            throw new InvitationClassReceiverOwnerIsAnUserSenderException(userSenderId, classReceiverId);
        }
    }
}