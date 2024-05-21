using System.Security.Cryptography;
using HiClass.Application.Common.Exceptions.Database;
using HiClass.Application.Interfaces;
using HiClass.Domain.Entities.Communication;
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
        var user = await _context.Users.SingleOrDefaultAsync(u => u.UserId == request.UserRecipientId, cancellationToken);

        if (user is null)
        {
            throw new UserNotFoundException(request.UserRecipientId);
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

        _context.Feedbacks.Add(feedback);

        var averageRating = await CalculateAverageRating(request.UserRecipientId, cancellationToken);

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