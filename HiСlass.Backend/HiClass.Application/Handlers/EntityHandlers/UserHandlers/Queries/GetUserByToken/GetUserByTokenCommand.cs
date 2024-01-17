using HiClass.Domain.Entities.Main;
using MediatR;

namespace HiClass.Application.Handlers.EntityHandlers.UserHandlers.Queries.GetUserByToken;

public class GetUserByTokenCommand : IRequest<User>
{
    public string Token { get; set; }
    public GetUserByTokenCommand(string token)
    {
        Token = token;
    }
}