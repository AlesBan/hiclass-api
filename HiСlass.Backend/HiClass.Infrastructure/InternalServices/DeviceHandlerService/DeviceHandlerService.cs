using HiClass.Application.Handlers.EntityHandlers.DeviceHandlers.Queries.GetAllUserDevicesByUserId;
using MediatR;

namespace HiClass.Infrastructure.InternalServices.DeviceHandlerService;

public class DeviceHandlerService : IDeviceHandlerService
{
    public Task<List<string>> GetActiveUserDeviceTokensByUserId(Guid userId, IMediator mediator)
    {
        var command = new GetActiveUserDevicesByUserIdCommand
        {
            UserId = userId
        };
        var deviceTokens = mediator.Send(command);
        return deviceTokens;
    }

}