using MediatR;

namespace HiClass.Infrastructure.InternalServices.DeviceHandlerService;

public interface IDeviceHandlerService
{
    Task<List<string>> GetActiveUserDeviceTokensByUserId(Guid userId, IMediator mediator);
}