using HiClass.Application.Models.EmailManager;
using HiClass.Application.Models.Invitation;
using HiClass.Application.Models.Invitation.Feedback;
using HiClass.Domain.Entities.Communication;
using MediatR;

namespace HiClass.Infrastructure.Services.InvitationServices;

public interface IInvitationService
{
    Task<Invitation> CreateInvitation(Guid userSenderId, IMediator mediator, CreateInvitationRequestDto requestInvitationDto);
    Task SendFeedback(Guid userSenderId, IMediator mediator, CreateFeedbackRequestDto sendFeedbackRequestDto);
}