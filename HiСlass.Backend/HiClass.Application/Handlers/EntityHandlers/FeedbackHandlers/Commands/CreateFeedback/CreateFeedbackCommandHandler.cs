using HiClass.Application.Interfaces;
using HiClass.Domain.Entities.Communication;
using MediatR;

namespace HiClass.Application.Handlers.EntityHandlers.FeedbackHandlers.Commands.CreateFeedback;

public class CreateFeedbackCommandHandler : IRequestHandler<CreateFeedbackCommand, Unit>
{
    private readonly ISharedLessonDbContext _context;

    public CreateFeedbackCommandHandler(ISharedLessonDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(CreateFeedbackCommand request, CancellationToken cancellationToken)
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
            Rating = request.Rating
        };

        _context.Feedbacks.Add(feedback);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}