using HiClass.Application.Models.User.Authentication;
using MediatR;

namespace HiClass.Application.Handlers.EntityHandlers.UserHandlers.Commands.RefreshToken;

public class RefreshTokenCommand : IRequest<TokenModelResponseDto>
{
    public Guid UserId { get; set; }
    public string RefreshToken { get; set; } = string.Empty;
    public string DeviceToken { get; set; } = string.Empty;
}