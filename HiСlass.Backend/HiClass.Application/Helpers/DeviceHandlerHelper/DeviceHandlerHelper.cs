using HiClass.Application.Handlers.EntityHandlers.DeviceHandlers.Commands.CreateDevice;
using HiClass.Application.Handlers.EntityHandlers.DeviceHandlers.Commands.DeleteDeviceByToken;
using HiClass.Application.Handlers.EntityHandlers.DeviceHandlers.Queries.GetDeviceByToken;
using HiClass.Domain.Entities.Notifications;
using MediatR;

namespace HiClass.Application.Helpers.DeviceHandlerHelper;

public class DeviceHandlerHelper : IDeviceHandlerHelper
{
    public Task<Device> GetDeviceByToken(string deviceToken, IMediator mediator)
    {
        var command = new GetDeviceByTokenQuery()
        {
            DeviceToken = deviceToken
        };

        var device = mediator.Send(command);
        return device;
    }

    public Task<Device> CreateDevice(string deviceToken, IMediator mediator)
    {
        var command = new CreateDeviceCommand()
        {
            DeviceToken = deviceToken
        };

        var device = mediator.Send(command);
        return device;
    }

    public Task DeleteDevice(string deviceToken, IMediator mediator)
    {
        var command = new DeleteDeviceByTokenCommand()
        {
            DeviceToken = deviceToken
        };

        mediator.Send(command);

        return Task.CompletedTask;
    }
}