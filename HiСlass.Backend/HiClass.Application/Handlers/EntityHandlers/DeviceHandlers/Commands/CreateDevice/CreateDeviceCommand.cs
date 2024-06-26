using HiClass.Domain.Entities.Notifications;
using MediatR;

namespace HiClass.Application.Handlers.EntityHandlers.DeviceHandlers.Commands.CreateDevice;

public class CreateDeviceCommand : IRequest<Unit>
{
    public string DeviceToken { get; set; } = string.Empty;

    public Guid UserId { get; set; }
}