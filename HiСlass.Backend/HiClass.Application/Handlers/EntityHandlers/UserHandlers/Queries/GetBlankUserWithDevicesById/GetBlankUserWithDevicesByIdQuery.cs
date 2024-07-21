using HiClass.Domain.Entities.Main;
using MediatR;

namespace HiClass.Application.Handlers.EntityHandlers.UserHandlers.Queries.GetBlankUserWithDevicesById;

public class GetBlankUserWithDevicesByIdQuery : IRequest<User>
{
    public Guid UserId { get; set; }

    public GetBlankUserWithDevicesByIdQuery(Guid userId)
    {
        UserId = userId;
    }
}