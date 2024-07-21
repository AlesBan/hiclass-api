using System.ComponentModel.DataAnnotations;
using HiClass.Application.Models.User.Authentication;
using HiClass.Application.Models.User.Login;
using MediatR;

namespace HiClass.Application.Handlers.EntityHandlers.UserHandlers.Commands.LoginUser;

public class LoginAndRefreshTokenCommand : IRequest<TokenModelResponseDto>
{
    [Required, EmailAddress] public string Email { get; set; } = string.Empty;
    [Required] public string Password { get; set; } = string.Empty;
    [Required] public string DeviceToken { get; set; } = string.Empty;
}