using HiClass.Application.Interfaces;
using HiClass.Domain.Entities.Communication;
using MediatR;

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
        await _context.SaveChangesAsync(cancellationToken);

        return feedback;
    }
}