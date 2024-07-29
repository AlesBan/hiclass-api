using System.ComponentModel.DataAnnotations;
using HiClass.Application.Models.User.Authentication;
using MediatR;

namespace HiClass.Application.Handlers.EntityHandlers.UserHandlers.Commands.LoginOrRegisterByEmailAndRefreshToken;

public class LoginOrRegisterByEmailAndRefreshTokenCommand : IRequest<TokenModelResponseDto>
{
    [Required] public string Email { get; set; } = string.Empty;

    [Required] public string DeviceToken { get; set; } = string.Empty;
}