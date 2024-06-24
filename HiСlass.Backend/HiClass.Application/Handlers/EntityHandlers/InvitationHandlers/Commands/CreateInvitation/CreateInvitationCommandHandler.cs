using HiClass.Application.Common.Exceptions.Class;
using HiClass.Application.Common.Exceptions.Invitations;
using HiClass.Application.Common.Exceptions.User;
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
        ValidateClassReceiverId(request.UserSenderId, request.ClassReceiverId);
        ValidateClassSenderId(request.UserSenderId, request.ClassSenderId);
        ValidateUserReceiverId(request.UserSenderId, request.UserReceiverId);

        var invitation = new Invitation
        {
            UserSenderId = request.UserSenderId,
            UserReceiverId = request.UserReceiverId,
            ClassSenderId =request.ClassSenderId,
            ClassReceiverId = request.ClassReceiverId,
            DateOfInvitation = request.DateOfInvitation.ToUniversalTime(),
            InvitationText = request.InvitationText,
            Status = request.Status,
            CreatedAt = DateTime.UtcNow
        };

        await _context.Invitations.AddAsync(invitation, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return invitation;
    }

    private void ValidateClassSenderId(Guid userSenderId, Guid classSenderId)
    {
        var classSenderIsUserSenderClass = _context.Classes
            .Any(c => c.ClassId == classSenderId && c.UserId == userSenderId);

        if (!classSenderIsUserSenderClass)
        {
            throw new InvitationClassSenderOwnerIsNotUserSenderException(userSenderId, classSenderId);
        }
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

    private async void ValidateUserReceiverId(Guid userSenderId, Guid userReceiverId)
    {
        var userReceiverIsExists = await _context.Users
            .FindAsync(userReceiverId);

        if (userReceiverIsExists == null)
        {
            throw new UserNotFoundByIdException(userSenderId, "UserSender was not found");
        }
    }
}