using HiClass.Application.Common.Exceptions.Database;
using HiClass.Application.Common.Exceptions.Invitations;
using HiClass.Application.Common.Exceptions.User;
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
        var invitation = await _context.Invitations
            .SingleOrDefaultAsync(i => i.InvitationId == request.InvitationId, cancellationToken);
        if (invitation is null)
        {
            throw new NotFoundException(nameof(Invitation), request.InvitationId);
        }

        var userFeedbackSenderId = request.UserFeedbackSenderId;
        var userFeedbackReceiverId = userFeedbackSenderId == invitation.UserSenderId
            ? invitation.UserRecipientId
            : invitation.UserSenderId;


        var user = await _context.Users.SingleOrDefaultAsync(u => u.UserId == userFeedbackReceiverId,
            cancellationToken);
        
        if (user is null)
        {
            throw new UserNotFoundByIdException(userFeedbackReceiverId);
        }

        switch (invitation.Status)
        {
            case var _ when invitation.Status == InvitationStatus.Declined:
                throw new InvitationIsNotAcceptedException(request.UserFeedbackSenderId, InvitationStatus.Declined);
            case var _ when invitation.Status == InvitationStatus.Pending:
                throw new InvitationIsNotAcceptedException(request.UserFeedbackSenderId, InvitationStatus.Pending);
        }

        var feedback = new Feedback()
        {
            UserFeedbackSenderId = userFeedbackSenderId,
            InvitationId = request.InvitationId,
            WasTheJointLesson = request.WasTheJointLesson,
            ReasonForNotConducting = request.ReasonForNotConducting,
            FeedbackText = request.FeedbackText,
            Rating = request.Rating,
            CreatedAt = DateTime.UtcNow
        };

        await _context.Feedbacks.AddAsync(feedback, cancellationToken);

        var averageRating = await CalculateAverageRating(userFeedbackReceiverId, cancellationToken);

        user.Rating = averageRating;
        _context.Users.Update(user);

        await _context.SaveChangesAsync(cancellationToken);

        return feedback;
    }


    private async Task<double> CalculateAverageRating(Guid userRecipientId, CancellationToken cancellationToken)
    {
        var ratings = await _context.Feedbacks
            .Where(f => f.UserFeedbackReceiverId == userRecipientId && f.Rating.HasValue)
            .Select(f => f.Rating!.Value)
            .ToListAsync(cancellationToken);

        if (ratings.Count == 0)
        {
            return 0;
        }

        return ratings.Average();
    }
}