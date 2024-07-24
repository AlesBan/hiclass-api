using HiClass.Domain.Entities.Main;
using MediatR;

namespace HiClass.Application.Handlers.EntityHandlers.UserHandlers.Queries.GetUserByEmail;

public class GetBlankUserByEmailQuery : IRequest<User>
{
    public string Email { get; set; }

    public GetBlankUserByEmailQuery(string email)
    {
        Email = email;
    }
}