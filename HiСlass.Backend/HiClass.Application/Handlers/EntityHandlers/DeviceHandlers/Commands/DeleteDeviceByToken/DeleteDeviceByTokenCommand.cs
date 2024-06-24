using MediatR;

namespace HiClass.Application.Handlers.EntityHandlers.DeviceHandlers.Commands.DeleteDeviceByToken;

public class DeleteDeviceByTokenCommand : IRequest<Unit>
{
    public string DeviceToken { get; set; }
}