using HiClass.Domain.Entities.Communication;
using MediatR;

namespace HiClass.Application.Handlers.EntityHandlers.FeedbackHandlers.Commands.CreateFeedback;

public class CreateFeedbackCommand : IRequest<Guid>
{
    public Guid InvitationId { get; set; }
    public bool WasTheJointLesson { get; set; }
    public string? ReasonForNotConducting { get; set; }
    public string? FeedbackText { get; set; }
    public int Rating { get; set; }
}