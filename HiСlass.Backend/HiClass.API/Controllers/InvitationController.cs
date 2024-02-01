using HiClass.API.Filters;
using HiClass.API.Filters.Abilities;
using HiClass.API.Helpers;
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

    public InvitationController(IInvitationService invitationService)
    {
        _invitationService = invitationService;
    }

    [HttpPost("create-invitation")]
    public async Task<IActionResult> CreateInvitation([FromBody] CreateInvitationRequestDto createInvitationRequestDto)
    {
        await _invitationService.CreateInvitation(UserId, Mediator, createInvitationRequestDto);
        return ResponseHelper.GetOkResult();
    }
}