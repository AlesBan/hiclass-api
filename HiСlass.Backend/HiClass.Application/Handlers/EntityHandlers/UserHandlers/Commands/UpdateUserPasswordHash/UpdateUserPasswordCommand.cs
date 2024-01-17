using HiClass.Domain.Entities.Main;
using MediatR;

namespace HiClass.Application.Handlers.EntityHandlers.UserHandlers.Commands.UpdateUserPasswordHash;

public class UpdateUserPasswordCommand : IRequest<User>
{
    public Guid UserId { get; set; }
    public string Password { get; set; }

    public UpdateUserPasswordCommand(Guid userId, string password)
    {
        UserId = userId;
        Password = password;
    }
}