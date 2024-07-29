using AutoMapper;
using HiClass.API.Filters.Abilities;
using HiClass.API.Filters.UserVerification;
using HiClass.API.Helpers;
using HiClass.API.Helpers.NotificationDtoCreatorHelper;
using HiClass.Application.Models.Invitations.CreateInvitation;
using HiClass.Application.Models.Invitations.Feedbacks.CreateFeedback;
using HiClass.Application.Models.Invitations.Invitations;
using HiClass.Application.Models.Invitations.UpdateInvitationStatus;
using HiClass.Domain.Enums;
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
    private readonly IMapper _mapper;
    private readonly INotificationDtoCreatorHelper _notificationDtoCreatorHelper;

    public InvitationController(IInvitationService invitationService,
        INotificationHandlerService notificationHandlerService,
        IMapper mapper, INotificationDtoCreatorHelper notificationDtoCreatorHelper)
    {
        _invitationService = invitationService;
        _notificationHandlerService = notificationHandlerService;
        _mapper = mapper;
        _notificationDtoCreatorHelper = notificationDtoCreatorHelper;
    }

    [CheckUserAbilityToSendInvitation]
    [HttpPost("send-class-invitation")]
    public async Task<IActionResult> SendClassInvitation([FromBody] CreateClassInvitationRequestDto requestDto)
    {
        var invitation = await _invitationService.CreateClassInvitation(UserId, Mediator, requestDto);

        var notificationDto = _notificationDtoCreatorHelper
            .CreateNotificationDto(invitation.UserRecipientId, NotificationType.Invitation,
                invitation.UserRecipient.Email);

        await _notificationHandlerService.ProcessNotification(notificationDto, Mediator);

        var invitationResponseDto = _mapper.Map<InvitationResponseDto>(invitation);
        return ResponseHelper.GetOkResult(invitationResponseDto);
    }
    
    [CheckUserAbilityToSendInvitation]
    [HttpPost("send-expert-invitation")]
    public async Task<IActionResult> SendExpertInvitation([FromBody] CreateExpertInvitationRequestDto requestDto)
    {
        var invitation = await _invitationService.CreateExpertInvitation(UserId, Mediator, requestDto);

        var notificationDto = _notificationDtoCreatorHelper
            .CreateNotificationDto(invitation.UserRecipientId, NotificationType.Invitation,
                invitation.UserRecipient.Email);

        await _notificationHandlerService.ProcessNotification(notificationDto, Mediator);

        var invitationResponseDto = _mapper.Map<InvitationResponseDto>(invitation);
        return ResponseHelper.GetOkResult(invitationResponseDto);
    }


    [HttpPost("update-invitation-status")]
    public async Task<IActionResult> UpdateInvitationStatus(
        [FromBody] UpdateInvitationStatusRequestDto updateInvitationStatusRequestDto)
    {
        await _invitationService.UpdateInvitationStatus(UserId, Mediator, updateInvitationStatusRequestDto);
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