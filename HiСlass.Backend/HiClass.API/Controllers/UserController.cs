using HiClass.API.Attributes;
using HiClass.API.Filters.Abilities;
using HiClass.API.Filters.UserVerification;
using HiClass.API.Helpers;
using HiClass.API.Helpers.JwtHelpers;
using HiClass.Application.Common.Exceptions.Authentication;
using HiClass.Application.Common.Exceptions.Database;
using HiClass.Application.Models.User;
using HiClass.Application.Models.User.Authentication;
using HiClass.Application.Models.User.CreateAccount;
using HiClass.Application.Models.User.EmailVerification;
using HiClass.Application.Models.User.EmailVerification.ReVerification;
using HiClass.Application.Models.User.PasswordHandling;
using HiClass.Infrastructure.InternalServices.UserServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace HiClass.API.Controllers;

[Produces("application/json")]
public class UserController : BaseController
{
    private readonly IUserAccountService _userAccountService;

    public UserController(IUserAccountService userAccountService)
    {
        _userAccountService = userAccountService;
    }

    [HttpGet("all-users")]
    public async Task<IActionResult> GetAllUsers()
    {
        var result = await _userAccountService.GetAllUsers(Mediator);
        return ResponseHelper.GetOkResult(result);
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserRegisterRequestDto requestUserDto)
    {
        var result = await _userAccountService.RegisterUser(requestUserDto, Mediator);
        return ResponseHelper.GetOkResult(result);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserLoginRequestDto requestUserDto)
    {
        var result = await _userAccountService.LoginUser(requestUserDto, Mediator);
        return ResponseHelper.GetOkResult(result);
    }

    [HttpPost("verify-email")]
    public async Task<IActionResult> VerifyEmail([FromBody] EmailVerificationRequestDto requestDto)
    {
        var email = requestDto.Email;
        var verificationCode = requestDto.VerificationCode;
        var result = await _userAccountService.VerifyEmailUsingEmail(email, verificationCode, Mediator);
        return ResponseHelper.GetOkResult(result);
    }

    [HttpPost("reverify-email")]
    public async Task<IActionResult> ReVerifyEmail([FromBody] EmailReVerificationRequestDto requestDto)
    {
        await _userAccountService.CreateAndReSendVerificationCode(requestDto, Mediator);
        return ResponseHelper.GetOkResult();
    }

    [Authorize]
    [CheckUserCreateAccountAbility]
    [HttpPut("create-account")]
    public async Task<IActionResult> CreateAccount([FromForm] CreateUserAccountRequestDto requestUserDto)
    {
        var result = await _userAccountService.CreateUserAccount(UserId, requestUserDto, Mediator);
        return ResponseHelper.GetOkResult(result);
    }

    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequestDto requestDto)
    {
        var email = requestDto.Email;
        var result = await _userAccountService.ForgotPassword(email, Mediator);
        return ResponseHelper.GetOkResult(result);
    }

    [HttpPost("check-reset-password-code")]
    public async Task<IActionResult> CheckResetPasswordCode([FromBody] CheckResetPasswordCodeDto resetPasswordCodeDto)
    {
        var resetCode = resetPasswordCodeDto.ResetCode;
        await _userAccountService.CheckResetPasswordCode(UserId, resetCode, Mediator);
        return ResponseHelper.GetOkResult();
    }

    [Authorize]
    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequestDto requestDto)
    {
        var result = await _userAccountService
            .ResetPassword(UserId, requestDto, Mediator);
        return ResponseHelper.GetOkResult(result);
    }

    [Authorize]
    [CheckUserVerification]
    [CheckUserCreateAccount]
    [HttpGet("userprofile")]
    public async Task<IActionResult> GetUser()
    {
        var userId = JwtHelper.GetUserIdFromClaims(HttpContext);
        var result = await _userAccountService.GetUserProfile(userId, Mediator);
        return ResponseHelper.GetOkResult(result);
    }

    [Authorize]
    [CheckUserVerification]
    [CheckUserCreateAccount]
    [HttpGet("other-userprofile/{userId:guid}")]
    public async Task<IActionResult> GetOtherUser([FromRoute] Guid userId)
    {
        var result = await _userAccountService.GetUserProfile(userId, Mediator);
        return ResponseHelper.GetOkResult(result);
    }

    [Authorize]
    [CheckUserVerification]
    [CheckUserCreateAccount]
    [HttpDelete("delete-user")]
    public async Task<IActionResult> DeleteUser()
    {
        await _userAccountService.DeleteUser(UserId, Mediator);
        return Ok();
    }

    [HttpDelete("delete-all-users")]
    public async Task<IActionResult> DeleteAllUsers()
    {
        await _userAccountService.DeleteAllUsers(Mediator);
        return Ok();
    }
}