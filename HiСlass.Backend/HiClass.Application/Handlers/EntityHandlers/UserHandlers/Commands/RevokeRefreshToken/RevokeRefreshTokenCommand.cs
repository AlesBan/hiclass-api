using MediatR;

namespace HiClass.Application.Handlers.EntityHandlers.UserHandlers.Commands.RevokeRefreshToken;

public class RevokeRefreshTokenCommand : IRequest
{
    public Guid UserId { get; set; }
    public string DeviceToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    
}