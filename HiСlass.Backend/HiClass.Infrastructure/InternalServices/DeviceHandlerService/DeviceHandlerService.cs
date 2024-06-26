using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using HiClass.Application.Handlers.EntityHandlers.DeviceHandlers.Commands.CreateDevice;
using HiClass.Application.Handlers.EntityHandlers.DeviceHandlers.Queries.GetAllUserDevicesByUserId;
using MediatR;

namespace HiClass.Infrastructure.InternalServices.DeviceHandlerService;

public class DeviceHandlerService : IDeviceHandlerService
{
    public Task<List<string>> GetUserDeviceTokensByUserId(Guid userId, IMediator mediator)
    {
        var command = new GetAllUserDevicesByUserIdCommand
        {
            UserId = userId
        };
        var deviceTokens = mediator.Send(command);
        return deviceTokens;
    }

    public async Task CreateDeviceByToken(Guid userId, string deviceToken, IMediator mediator)
    {
        var command = new CreateDeviceCommand
        {
            UserId = userId,
            DeviceToken = deviceToken
        };
        await mediator.Send(command);
    }
}