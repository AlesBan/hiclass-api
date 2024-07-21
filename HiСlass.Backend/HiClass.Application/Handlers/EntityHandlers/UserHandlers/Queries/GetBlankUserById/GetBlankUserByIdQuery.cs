using HiClass.Domain.Entities.Main;
using MediatR;

namespace HiClass.Application.Handlers.EntityHandlers.UserHandlers.Queries.GetBlankUserById;

public class GetBlankUserByIdQuery : IRequest<User>
{
    public Guid UserId { get; set; }

    public GetBlankUserByIdQuery(Guid userId)
    {
        UserId = userId;
    }
}