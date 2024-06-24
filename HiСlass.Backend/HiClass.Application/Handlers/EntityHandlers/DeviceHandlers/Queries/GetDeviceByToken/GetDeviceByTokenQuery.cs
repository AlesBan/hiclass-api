using HiClass.Domain.Entities.Notifications;
using MediatR;

namespace HiClass.Application.Handlers.EntityHandlers.DeviceHandlers.Queries.GetDeviceByToken;

public class GetDeviceByTokenQuery : IRequest<Device>
{
    public string DeviceToken { get; set; }
}