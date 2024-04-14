using HiClass.Application.Models.Invitations.CreateInvitation;
using HiClass.Application.Models.Invitations.Feedbacks.CreateFeedback;
using HiClass.Domain.Entities.Communication;
using MediatR;

namespace HiClass.Infrastructure.Services.InvitationServices;

public interface IInvitationService
{
    Task<Invitation> CreateInvitation(Guid userSenderId, IMediator mediator, CreateInvitationRequestDto requestInvitationDto);
    Task<Feedback> CreateFeedback(Guid userSenderId, IMediator mediator, CreateFeedbackRequestDto sendFeedbackRequestDto);
}