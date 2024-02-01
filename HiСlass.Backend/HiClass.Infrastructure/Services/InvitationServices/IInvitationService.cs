using HiClass.Application.Models.Invitation;
using MediatR;

namespace HiClass.Infrastructure.Services.InvitationServices;

public interface IInvitationService
{
    Task CreateInvitation(Guid userSenderId, IMediator mediator, CreateInvitationRequestDto requestInvitationDto);
}