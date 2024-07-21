using MediatR;

namespace HiClass.Application.Handlers.EntityConnectionHandlers.UserDeviceHandlers.Commands.CreateUserDevice;

public class CreateUserDeviceCommand : IRequest<Unit>
{
    public string DeviceToken { get; set; } = string.Empty;
    public Guid UserId { get; set; }
    public string RefreshToken { get; set; } = string.Empty;
}