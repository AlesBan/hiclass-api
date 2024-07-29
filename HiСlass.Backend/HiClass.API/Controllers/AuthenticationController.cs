using FirebaseAdmin.Auth;
using HiClass.API.Helpers;
using HiClass.Application.Common.Exceptions.Firebase;
using HiClass.Application.Models.User.Authentication;
using HiClass.Application.Models.User.PasswordHandling;
using HiClass.Application.Models.User.RevokeToken;
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
    
    [HttpPost("google-signin")]
    public async Task<IActionResult> GoogleSignIn([FromBody] GoogleSingInRequestDto requestDto)
    {
        try
        {
            var decodedToken = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(requestDto.Token);
            var email = decodedToken.Claims["email"].ToString();
            
            if (string.IsNullOrEmpty(email))
            {
                throw new InvalidFirebaseTokenProvidedException();
            }

            var result = await _userAccountService.LoginOrRegister(email, requestDto.Token, Mediator);
            return ResponseHelper.GetOkResult(result);
        }
        catch
        {
            return Unauthorized();
        }
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
        await _userAccountService.GenerateAndSetResetPasswordCodeAsync(email, Mediator);
        return ResponseHelper.GetOkResult();
    }

    [HttpPost("check-reset-password-code")]
    public async Task<IActionResult> CheckResetPasswordCode([FromBody] CheckResetPasswordCodeDto requestDto)
    {
        var userEmail = requestDto.Email;
        var resetCode = requestDto.ResetCode;
        var result = await _userAccountService.CheckResetPasswordCodeAsync(userEmail, resetCode, Mediator);
        return ResponseHelper.GetOkResult(result);
    }

    [Authorize]
    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequestDto requestDto)
    {
        var result = await _userAccountService
            .ResetPasswordAsync(UserId, requestDto, Mediator);
        return ResponseHelper.GetOkResult(result);
    }


    [Authorize]
    [HttpPost]
    [Route("revoke")]
    public async Task<IActionResult> Revoke([FromBody] RevokeTokenRequestDto requestDto)
    {
        await _userAccountService.RevokeRefreshTokenAsync(UserId, requestDto, Mediator);
        return ResponseHelper.GetOkResult();
    }
}