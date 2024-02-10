using HiClass.API.Filters.Abilities;
using HiClass.API.Helpers;
using HiClass.Application.Interfaces.Services;
using HiClass.Application.Models.User.Update;
using HiClass.Infrastructure.Services.UpdateUserAccountService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HiClass.API.Controllers;

[Authorize]
[CheckUserCreateAccount]
public class UpdateUserController : BaseController
{
    private readonly IUpdateUserAccountService _editUserAccountService;

    public UpdateUserController(IUpdateUserAccountService editUserAccountService)
    {
        _editUserAccountService = editUserAccountService;
    }

    /// <summary>
    /// Update user personal info
    /// </summary>
    /// <remarks>
    /// PUT /api/edituser/personal-info
    /// </remarks>
    /// <param name="requestUserDto">UpdatePersonalInfoRequestDto object</param>
    /// <returns>
    /// Returns UserProfileDto object
    /// </returns>
    /// <response code="200">Returns UserProfileDto object</response>
    /// <response code="400">Returns ResponseResult object</response>
    /// <response code="401">Returns Unauthorized object</response>
    [HttpPut("personal-info")]
    public async Task<IActionResult> EditUserPersonalInfo([FromBody] UpdatePersonalInfoRequestDto requestUserDto)
    {
        var result = await _editUserAccountService.UpdateUserPersonalInfoAsync(UserId, requestUserDto, Mediator);
        return ResponseHelper.GetOkResult(result);
    }

    [HttpPut("image")]
    public async Task<IActionResult> EditUserImage([FromForm] UpdateImageRequestDto requestDto)
    {
        var result = await _editUserAccountService.UpdateUserImageAsync(UserId, requestDto, Mediator);
        return ResponseHelper.GetOkResult(result);
    }

    /// <summary>
    /// Update user institution
    /// </summary>
    /// <remarks>
    /// PUT /api/edituser/institution
    /// </remarks>
    /// <param name="requestUserDto">UpdateInstitutionRequestDto object</param>
    /// <returns>
    /// Returns UserProfileDto object
    /// </returns>
    /// <response code="200">Returns UserProfileDto object</response>
    /// <response code="400">Returns ResponseResult object</response>
    /// <response code="401">Returns Unauthorized object</response>
    [HttpPut("institution")]
    public async Task<IActionResult> EditUserInstitution([FromBody] UpdateInstitutionRequestDto requestUserDto)
    {
        var result = await _editUserAccountService.UpdateUserInstitutionAsync(UserId, requestUserDto, Mediator);
        return ResponseHelper.GetOkResult(result);
    }

    /// <summary>
    /// Update user professional info
    /// </summary>
    /// <remarks>
    /// PUT /api/edituser/professional-info
    /// </remarks>
    /// <param name="requestUserDto">UpdateProfessionalInfoRequestDto object</param>
    /// <returns>
    /// Returns UserProfileDto object
    /// </returns>
    /// <response code="200">Returns UserProfileDto object</response>
    /// <response code="400">Returns ResponseResult object</response>
    /// <response code="401">Returns Unauthorized object</response>
    [HttpPut("professional-info")]
    public async Task<IActionResult> EditUserProfessionalInfo(
        [FromBody] UpdateProfessionalInfoRequestDto requestUserDto)
    {
        var result = await _editUserAccountService.UpdateUserProfessionalInfoAsync(UserId, requestUserDto, Mediator);
        return ResponseHelper.GetOkResult(result);
    }

    /// <summary>
    /// Update user email
    /// </summary>
    /// <remarks>
    /// PUT /api/edituser/email
    /// </remarks>
    /// <param name="requestUserDto">UpdateUserEmailRequestDto object</param>
    /// <returns>
    /// Returns UserProfileDto object
    /// </returns>
    /// <response code="200">Returns UserProfileDto object</response>
    /// <response code="400">Returns ResponseResult object</response>
    /// <response code="401">Returns Unauthorized object</response>
    [HttpPut("email")]
    public async Task<IActionResult> EditUserEmail([FromBody] UpdateUserEmailRequestDto requestUserDto)
    {
        var result = await _editUserAccountService.UpdateUserEmailAsync(UserId, requestUserDto, Mediator);
        return ResponseHelper.GetOkResult(result);
    }

    /// <summary>
    /// Update user password
    /// </summary>
    /// <remarks>
    /// PUT /api/edituser/password
    /// </remarks>
    /// <param name="requestUserDto">UpdateUserPasswordRequestDto object</param>
    /// <returns>
    /// Returns UserProfileDto object
    /// </returns>
    /// <response code="200">Returns UserProfileDto object</response>
    /// <response code="400">Returns ResponseResult object</response>
    /// <response code="401">Returns Unauthorized object</response>
    [HttpPut("password")]
    public async Task<IActionResult> EditUserPassword([FromBody] UpdateUserPasswordHashRequestDto requestUserDto)
    {
        var result = await _editUserAccountService.UpdateUserPasswordAsync(UserId, requestUserDto, Mediator);
        return ResponseHelper.GetOkResult(result);
    }
}