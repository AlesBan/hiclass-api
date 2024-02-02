using HiClass.API.Filters.Abilities;
using HiClass.API.Helpers;
using HiClass.Application.Models.EmailManager;
using HiClass.Application.Models.Invitation;
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
        var emailCredentials = new EmailManagerCredentials()
        {
            Email =_configuration["EMAIL_MANAGER:EMAIL"],
            Password = _configuration["EMAIL_MANAGER:PASSWORD"]
        };

        await _invitationService.CreateInvitation(emailCredentials, UserId, Mediator, createInvitationRequestDto);
        return ResponseHelper.GetOkResult();
    }
}