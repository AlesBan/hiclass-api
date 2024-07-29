using HiClass.Application.Common.Exceptions.Database;
using HiClass.Application.Common.Exceptions.Invitations;
using HiClass.Application.Common.Exceptions.User;
using HiClass.Application.Interfaces;
using HiClass.Domain.Entities.Communication;
using HiClass.Domain.Entities.Main;
using MediatR;
using Microsoft.EntityFrameworkCore;

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
        await ValidateUserReceiverId(request.UserSenderId, request.UserReceiverId, request.ClassReceiverId);
        await ValidateClassReceiverId(request.UserSenderId, request.ClassReceiverId);
        await ValidateClassSenderId(request.UserSenderId, request.ClassSenderId);

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

        await Task.Delay(30, cancellationToken);

        await _context.Invitations.AddAsync(invitation, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        await Task.Delay(20, cancellationToken);

        var savedInvitation = await _context.Invitations
            .Include(x => x.UserReceiver)
            .Include(x => x.UserSender)
            .Include(x => x.ClassReceiver)
            .Include(x => x.ClassSender)
            .FirstOrDefaultAsync(x => x.InvitationId == invitation.InvitationId, cancellationToken);

        return savedInvitation!;
    }

    private async Task ValidateClassSenderId(Guid userSenderId, Guid classSenderId)
    {
        var classSenderIsUserSenderClass = await _context.Classes
            .FindAsync(classSenderId);
        
        if (classSenderIsUserSenderClass == null)
        {
            throw new NotFoundException(nameof(Class), classSenderId);
        }

        if (classSenderIsUserSenderClass.UserId != userSenderId)
        {
            throw new ClassNotOwnedByUserException(userSenderId, classSenderId);
        }
    }

    private async Task ValidateClassReceiverId(Guid userSenderId, Guid classReceiverId)
    {
        var classReceiverIsUserSenderClass = await _context.Classes
            .FirstOrDefaultAsync(c => c.ClassId == classReceiverId);

        if (classReceiverIsUserSenderClass == null)
        {
            throw new NotFoundException(nameof(Class), classReceiverId);
        }

        if (classReceiverIsUserSenderClass.UserId == userSenderId)
        {
            throw new InvitationReceiverIsSenderException(userSenderId, classReceiverId);
        }
    }


    private async Task ValidateUserReceiverId(Guid userSenderId, Guid userReceiverId, Guid classReceiverId)
    {
        var userReceiver = await _context.Users
            .FindAsync(userReceiverId);

        if (userReceiver == null)
        {
            throw new UserNotFoundByIdException(userSenderId, "UserSender was not found");
        }

        if (userReceiver.IsATeacher != true)
        {
           throw new InvalidUserPositionForClassReceiverInvitationException(userSenderId, userReceiverId, classReceiverId);
        }
    }
}