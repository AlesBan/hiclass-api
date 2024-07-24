using HiClass.API.Helpers;
using HiClass.Application.Models.User.Authentication;
using HiClass.Application.Models.User.PasswordHandling;
using HiClass.Infrastructure.InternalServices.UserServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HiClass.API.Controllers;

public class AuthenticationController : BaseController
{
    private readonly IUserAccountService _userAccountService;

    public AuthenticationController(IUserAccountService userAccountService)
    {
        _userAccountService = userAccountService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequestDto requestDto)
    {
        var result = await _userAccountService.Register(requestDto, Mediator);
        return ResponseHelper.GetOkResult(result);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto requestDto)
    {
        var result = await _userAccountService.Login(requestDto, Mediator);
        return ResponseHelper.GetOkResult(result);
    }
    
    [Authorize]
    [HttpPost("log-out")]
    public async Task<IActionResult> LogOut([FromBody] LogOutRequestDto requestDto)
    {
        await _userAccountService.LogOut(UserId, requestDto, Mediator);
        return ResponseHelper.GetOkResult();
    }

    [HttpPost]
    [Route("refresh-token")]
    public async Task<IActionResult> RefreshToken(RefreshTokenRequestDto refreshTokenRequestDto)
    {
        var result = await _userAccountService.RefreshToken(UserId, refreshTokenRequestDto, Mediator);

        return ResponseHelper.GetOkResult(result);
    }

    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequestDto requestDto)
    {
        var email = requestDto.Email;
        await _userAccountService.ForgotPassword(email, Mediator);
        return ResponseHelper.GetOkResult();
    }

    [HttpPost("check-reset-password-code")]
    public async Task<IActionResult> CheckResetPasswordCode([FromBody] CheckResetPasswordCodeDto requestDto)
    {
        var userEmail = requestDto.Email;
        var resetCode = requestDto.ResetCode;
        var result = await _userAccountService.CheckResetPasswordCode(userEmail, resetCode, Mediator);
        return ResponseHelper.GetOkResult(result);
    }

    [Authorize]
    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequestDto requestDto)
    {
        var result = await _userAccountService
            .ResetPassword(UserId, requestDto, Mediator);
        return ResponseHelper.GetOkResult(result);
    }


    // [Authorize]
    // [HttpPost]
    // [Route("revoke")]
    // public async Task<IActionResult> Revoke()
    // {
    //     var user = await _userHelper.GetBlankUserById(UserId, Mediator);
    //
    //     user.RefreshToken = null;
    //     await _userHelper.UpdateAsync(user);
    //
    //     return ResponseHelper.GetNoContentResult();
    //
    // }
}