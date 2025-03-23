using System.ComponentModel.DataAnnotations;
using HiClass.Application.Models.User.Authentication;
using MediatR;

namespace HiClass.Application.Handlers.Authentication.RefreshTokenHandlers.Commands;

public class RefreshTokenCommand : IRequest<TokenModelResponseDto>
{
    [Required] public Guid UserId { get; set; }
    [Required] public string RefreshToken { get; set; } = string.Empty;
    public string DeviceToken { get; set; } = string.Empty;
}