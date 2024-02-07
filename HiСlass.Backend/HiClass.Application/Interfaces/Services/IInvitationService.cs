using HiClass.Application.Models.EmailManager;
using HiClass.Application.Models.Invitation;
using MediatR;

namespace HiClass.Application.Interfaces.Services;

public interface IInvitationService
{
    Task CreateInvitation(EmailManagerCredentials credentials, Guid userSenderId, IMediator mediator, CreateInvitationRequestDto requestInvitationDto);
}