using HiClass.Domain.Entities.Communication;
using MediatR;

namespace HiClass.Application.Handlers.EntityHandlers.FeedbackHandlers.Commands.DeleteFeedback;

public class DeleteFeedbackCommand : IRequest<Unit>
{
    public Review Feedback { get; set; }
}