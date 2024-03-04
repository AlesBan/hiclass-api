using HiClass.API.Filters.Abilities;
using HiClass.API.Helpers;
using HiClass.Application.Interfaces.Services;
using HiClass.Application.Models.EmailManager;
using HiClass.Application.Models.Invitation;
using HiClass.Application.Models.Invitation.Feedback;
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
        var emailCredentials = new EmailManagerCredentials(_configuration["EMAIL_MANAGER:EMAIL"],
            _configuration["EMAIL_MANAGER:PASSWORD"]);

        await _invitationService.CreateInvitation(emailCredentials, UserId, Mediator, createInvitationRequestDto);
        return ResponseHelper.GetOkResult();
    }

    [HttpPost("send-feedback")]
    public async Task<IActionResult> SendFeedback([FromBody] CreateFeedbackRequestDto sendFeedbackRequestDto)
    {
        await _invitationService.SendFeedback(UserId, Mediator, sendFeedbackRequestDto);
        return ResponseHelper.GetOkResult();
    }
}