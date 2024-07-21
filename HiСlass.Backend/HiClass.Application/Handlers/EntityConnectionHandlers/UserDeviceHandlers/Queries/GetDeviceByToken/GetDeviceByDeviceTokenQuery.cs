using HiClass.Domain.Entities.Notifications;
using MediatR;

namespace HiClass.Application.Handlers.EntityConnectionHandlers.UserDeviceHandlers.Queries.GetDeviceByToken;

public class GetDeviceByDeviceTokenQuery : IRequest<Device>
{
    public string DeviceToken { get; set; } = string.Empty;
}