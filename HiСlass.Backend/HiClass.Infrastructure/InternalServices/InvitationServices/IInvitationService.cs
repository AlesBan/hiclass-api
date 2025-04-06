using HiClass.Application.Models.Invitations.CreateInvitation;
using HiClass.Application.Models.Invitations.Feedbacks.CreateFeedback;
using HiClass.Application.Models.Invitations.UpdateInvitationStatus;
using HiClass.Domain.Entities.Communication;
using MediatR;

namespace HiClass.Infrastructure.InternalServices.InvitationServices;

public interface IInvitationService
{
    Task<Invitation> CreateClassInvitationAndSendEmailInvitation(Guid userSenderId, IMediator mediator,
        CreateClassInvitationRequestDto requestInvitationDto);
    Task<Invitation> CreateExpertInvitationAndSendEmailInvitation(Guid userSenderId, IMediator mediator,
        CreateExpertInvitationRequestDto requestInvitationDto);
    Task UpdateInvitationStatus(Guid userReceiverId, IMediator mediator,
        UpdateInvitationStatusRequestDto requestDto);
    Task<Feedback> CreateFeedback(Guid userSenderId, IMediator mediator,
        CreateFeedbackRequestDto sendFeedbackRequestDto);
}