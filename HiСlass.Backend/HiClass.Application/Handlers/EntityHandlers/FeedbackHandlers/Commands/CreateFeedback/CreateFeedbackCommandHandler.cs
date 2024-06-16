using System.Security.Cryptography;
using HiClass.Application.Common.Exceptions.Database;
using HiClass.Application.Common.Exceptions.Invitations;
using HiClass.Application.Interfaces;
using HiClass.Domain.Entities.Communication;
using HiClass.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HiClass.Application.Handlers.EntityHandlers.FeedbackHandlers.Commands.CreateFeedback;

public class CreateFeedbackCommandHandler : IRequestHandler<CreateFeedbackCommand, Feedback>
{
    private readonly ISharedLessonDbContext _context;

    public CreateFeedbackCommandHandler(ISharedLessonDbContext context)
    {
        _context = context;
    }

    public async Task<Feedback> Handle(CreateFeedbackCommand request, CancellationToken cancellationToken)
    {
        var userTask = _context.Users.SingleOrDefaultAsync(u => u.UserId == request.UserRecipientId, cancellationToken);
        var invitationTask =
            _context.Invitations.SingleOrDefaultAsync(i => i.InvitationId == request.InvitationId, cancellationToken);

        var user = await userTask;
        if (user is null)
        {
            throw new UserNotFoundByIdException(request.UserRecipientId);
        }

        var invitation = await invitationTask;
        if (invitation is null)
        {
            throw new NotFoundException(nameof(Invitation), request.InvitationId);
        }

        switch (invitation.Status)
        {
            case var _ when invitation.Status == InvitationStatus.Declined.ToString():
                throw new InvitationIsNotAcceptedException(InvitationStatus.Declined);
            case var _ when invitation.Status == InvitationStatus.Pending.ToString():
                throw new InvitationIsNotAcceptedException(InvitationStatus.Pending);
        }

        var feedback = new Feedback()
        {
            UserSenderId = request.UserSenderId,
            UserRecipientId = request.UserRecipientId,
            ClassSenderId = request.ClassSenderId,
            ClassReceiverId = request.ClassReceiverId,
            InvitationId = request.InvitationId,
            WasTheJointLesson = request.WasTheJointLesson,
            ReasonForNotConducting = request.ReasonForNotConducting,
            FeedbackText = request.FeedbackText,
            Rating = request.Rating,
            CreatedAt = DateTime.UtcNow
        };

        await _context.Feedbacks.AddAsync(feedback, cancellationToken);

        var averageRating = await CalculateAverageRating(request.UserRecipientId, cancellationToken);

        user.Rating = averageRating;
        _context.Users.Update(user);

        await _context.SaveChangesAsync(cancellationToken);

        return feedback;
    }


    private async Task<double> CalculateAverageRating(Guid userRecipientId, CancellationToken cancellationToken)
    {
        var ratings = await _context.Feedbacks
            .Where(f => f.UserRecipientId == userRecipientId && f.Rating.HasValue)
            .Select(f => f.Rating!.Value)
            .ToListAsync(cancellationToken);

        if (ratings.Count == 0)
        {
            return 0;
        }

        return ratings.Average();
    }
}