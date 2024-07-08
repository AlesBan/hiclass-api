using MediatR;

namespace HiClass.Infrastructure.InternalServices.DeviceHandlerService;

public interface IDeviceHandlerService
{
    Task<List<string>> GetUserDeviceTokensByUserId(Guid userId, IMediator mediator);
    Task CreateDeviceByToken(Guid userId, string deviceToken, IMediator mediator);
}