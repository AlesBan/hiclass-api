using AutoMapper;
using HiClass.API.Filters.Abilities;
using HiClass.API.Filters.UserVerification;
using HiClass.API.Helpers;
using HiClass.Application.Models.Invitations.ChangeInvitationStatus;
using HiClass.Application.Models.Invitations.CreateInvitation;
using HiClass.Application.Models.Invitations.Feedbacks.CreateFeedback;
using HiClass.Application.Models.Invitations.Invitations;
using HiClass.Application.Models.Notifications;
using HiClass.Domain.Entities.Notifications;
using HiClass.Domain.Enums;
using HiClass.Infrastructure.InternalServices.DeviceHandlerService;
using HiClass.Infrastructure.InternalServices.InvitationServices;
using HiClass.Infrastructure.InternalServices.NotificationHandlerService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HiClass.API.Controllers;

[Authorize, CheckUserVerification, CheckUserCreateAccount]
public class InvitationController : BaseController
{
    private readonly IInvitationService _invitationService;
    private readonly INotificationHandlerService _notificationHandlerService;
    private readonly IDeviceHandlerService _deviceHandlerService;
    private readonly IMapper _mapper;

    public InvitationController(IInvitationService invitationService,
        INotificationHandlerService notificationHandlerService, IDeviceHandlerService deviceHandlerService,
        IMapper mapper)
    {
        _invitationService = invitationService;
        _notificationHandlerService = notificationHandlerService;
        _deviceHandlerService = deviceHandlerService;
        _mapper = mapper;
    }

    [HttpPost("create-invitation")]
    public async Task<IActionResult> CreateInvitation([FromBody] CreateInvitationRequestDto createInvitationRequestDto)
    {
        var invitation = await _invitationService.CreateInvitation(UserId, Mediator, createInvitationRequestDto);

        var notificationDto = new NotificationDto
        {
            UserReceiverId = invitation.UserReceiverId,
            NotificationType = NotificationType.Invitation,
            NotificationMessage = new NotificationMessage
            {
                Sender = invitation.UserSender.Email,
                Message = $"{invitation.UserSender.Email} sent you an invitation to join his/her class"
            }
        };
        
        var notification = await _notificationHandlerService.CreateNotification(notificationDto, Mediator);
        
        var userDeviceTokens =
            await _deviceHandlerService.GetUserDeviceTokensByUserId(invitation.UserReceiverId, Mediator);
        
        var notificationResponseDto = new NotificationResponseDto
        {
            NotificationType = notification.NotificationType.ToString(),
            NotificationMessage = notificationDto.NotificationMessage,
            IsRead = false,
            CreatedAt = default,
            DeviceTokens = userDeviceTokens
        };
        
        await _notificationHandlerService.SendNotificationAsync(notificationResponseDto, userDeviceTokens);

        var invitationResponseDto = _mapper.Map<InvitationResponseDto>(invitation);
        return ResponseHelper.GetOkResult(invitationResponseDto);
    }

    [HttpPost("change-invitation-status")]
    public async Task<IActionResult> ChangeInvitationStatus(
        [FromBody] ChangeInvitationStatusRequestDto changeInvitationStatusRequestDto)
    {
        await _invitationService.ChangeInvitationStatus(UserId, Mediator, changeInvitationStatusRequestDto);
        return ResponseHelper.GetOkResult();
    }

    [HttpPost("send-feedback")]
    public async Task<IActionResult> SendFeedback([FromBody] CreateFeedbackRequestDto sendFeedbackRequestDto)
    {
        var feedback = await _invitationService.CreateFeedback(UserId, Mediator, sendFeedbackRequestDto);
        var feedbackDto = new CreateFeedbackResponseDto(feedback);
        return ResponseHelper.GetOkResult(feedbackDto);
    }
}