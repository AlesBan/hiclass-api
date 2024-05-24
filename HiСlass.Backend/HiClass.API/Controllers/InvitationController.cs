using HiClass.API.Filters.Abilities;
using HiClass.API.Helpers;
using HiClass.Application.Models.Invitations.ChangeInvitationStatus;
using HiClass.Application.Models.Invitations.CreateInvitation;
using HiClass.Application.Models.Invitations.Feedbacks.CreateFeedback;
using HiClass.Infrastructure.Services.InvitationServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HiClass.API.Controllers;

[Authorize]
[CheckUserCreateAccount]
public class InvitationController : BaseController
{
    private readonly IInvitationService _invitationService;
    private readonly IConfiguration _configuration;

    public InvitationController(IInvitationService invitationService, IConfiguration configuration)
    {
        _invitationService = invitationService;
        _configuration = configuration;
    }
    

    [HttpPost("create-invitation")]
    public async Task<IActionResult> CreateInvitation([FromBody] CreateInvitationRequestDto createInvitationRequestDto)
    {
        var invitation = await _invitationService.CreateInvitation(UserId, Mediator, createInvitationRequestDto);
        var invitationDto = new CreateInvitationResponseDto(invitation);
        return ResponseHelper.GetOkResult(invitationDto);
    }
    
    [HttpPost("change-invitation-status")]
    public async Task<IActionResult> ChangeInvitationStatus([FromBody] ChangeInvitationStatusRequestDto changeInvitationStatusRequestDto)
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