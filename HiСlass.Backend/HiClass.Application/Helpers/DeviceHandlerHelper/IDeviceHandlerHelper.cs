using HiClass.Domain.Entities.Notifications;
using MediatR;

namespace HiClass.Application.Helpers.DeviceHandlerHelper;

public interface IDeviceHandlerHelper
{
    Task<Device> GetDeviceByToken(string deviceToken, IMediator mediator);

    Task<Device> CreateDevice(string deviceToken, IMediator mediator);

    Task DeleteDevice(string deviceToken, IMediator mediator);
}