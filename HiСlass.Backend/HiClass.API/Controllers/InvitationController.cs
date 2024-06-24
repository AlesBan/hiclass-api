using HiClass.API.Filters.Abilities;
using HiClass.API.Filters.UserVerification;
using HiClass.API.Helpers;
using HiClass.Application.Models.Invitations.ChangeInvitationStatus;
using HiClass.Application.Models.Invitations.CreateInvitation;
using HiClass.Application.Models.Invitations.Feedbacks.CreateFeedback;
using HiClass.Application.Models.Notifications;
using HiClass.Domain.Entities.Notifications;
using HiClass.Domain.Enums;
using HiClass.Infrastructure.Services.InvitationServices;
using HiClass.Infrastructure.Services.NotificationHandlerService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HiClass.API.Controllers;

[Authorize, CheckUserVerification, CheckUserCreateAccount]
public class InvitationController : BaseController
{
    private readonly IInvitationService _invitationService;
    private readonly INotificationHandlerService _notificationHandlerService;

    public InvitationController(IInvitationService invitationService,
        INotificationHandlerService notificationHandlerService)
    {
        _invitationService = invitationService;
        _notificationHandlerService = notificationHandlerService;
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
        
        var notificationResponseDto = new NotificationResponseDto
        {
            NotificationType = notification.NotificationType.ToString(),
            Message = notification.Message,
            IsRead = false,
            CreatedAt = default
        };
        
        //TODO: Получение device token'ов
        
        //TODO: Отправка уведомления

        
        var invitationDto = new CreateInvitationResponseDto(invitation);
        return ResponseHelper.GetOkResult(invitationDto);
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