using MediatR;

namespace HiClass.Application.Handlers.EntityHandlers.UserHandlers.Commands.LogOutUser;

public class LogOutUserCommand : IRequest
{
    public Guid UserId { get; set; }
    public string DeviceToken { get; set; } = string.Empty;
}