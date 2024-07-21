using HiClass.Domain.Entities.Main;
using MediatR;

namespace HiClass.Application.Handlers.EntityHandlers.UserHandlers.Queries.GetFullUserById;

public class GetFullUserByIdQuery : IRequest<User>
{
    public Guid UserId { get; set; }
    
    public GetFullUserByIdQuery(Guid userId)
    {
        UserId = userId;
    }
}