using MediatR;

namespace HiClass.Application.Handlers.EntityHandlers.DeviceHandlers.Queries.GetAllUserDevicesByUserId;

public class GetActiveUserDevicesByUserIdCommand : IRequest<List<string>>
{
    public Guid UserId { get; set; }
}