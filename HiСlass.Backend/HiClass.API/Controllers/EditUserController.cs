using HiClass.API.Filters.Abilities;
using HiClass.API.Helpers;
using HiClass.Application.Models.User.Editing;
using HiClass.Application.Models.User.Editing.Requests;
using HiClass.Infrastructure.InternalServices.EditUserAccountService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HiClass.API.Controllers;

[Authorize]
[CheckUserCreateAccount]
public class EditUserController : BaseController
{
    private readonly IEditUserAccountService _editUserAccountService;

    public EditUserController(IEditUserAccountService editUserAccountService)
    {
        _editUserAccountService = editUserAccountService;
    }

    [HttpPut("personal-info")]
    public async Task<IActionResult> EditUserPersonalInfo([FromBody] EditPersonalInfoRequestDto requestUserDto)
    {
        var result = await _editUserAccountService.EditUserPersonalInfoAsync(UserId, requestUserDto, Mediator);
        return ResponseHelper.GetOkResult(result);
    }

    [HttpPut("institution")]
    public async Task<IActionResult> EditUserInstitution([FromBody] EditInstitutionRequestDto requestUserDto)
    {
        var result = await _editUserAccountService.EditUserInstitutionAsync(UserId, requestUserDto, Mediator);
        return ResponseHelper.GetOkResult(result);
    }
    
    [HttpPut("professional-info")]
    public async Task<IActionResult> EditUserProfessionalInfo(
        [FromBody] EditProfessionalInfoRequestDto requestUserDto)
    {
        var result = await _editUserAccountService.EditUserProfessionalInfoAsync(UserId, requestUserDto, Mediator);
        return ResponseHelper.GetOkResult(result);
    }
    
    [HttpPut("email")]
    public async Task<IActionResult> EditUserEmail([FromBody] EditUserEmailRequestDto requestUserDto)
    {
        var result = await _editUserAccountService.EditUserEmailAsync(UserId, requestUserDto, Mediator);
        return ResponseHelper.GetOkResult(result);
    }

    [HttpPut("password")]
    public async Task<IActionResult> EditUserPassword([FromBody] EditUserPasswordHashRequestDto requestUserDto)
    {
        var result = await _editUserAccountService.EditUserPasswordAsync(UserId, requestUserDto, Mediator);
        return ResponseHelper.GetOkResult(result);
    }
    
    [HttpPut("set-password")]
    public async Task<IActionResult> SetUserPassword([FromBody] SetUserPasswordHashRequestDto requestUserDto)
    {
        var result = await _editUserAccountService.SetUserPasswordAsync(UserId, requestUserDto, Mediator);
        return ResponseHelper.GetOkResult(result);
    }
}