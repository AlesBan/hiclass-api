using MediatR;

namespace HiClass.Application.Handlers.EntityHandlers.UserHandlers.Commands.DeleteUser;

public class DeleteUserCommand : IRequest<Unit>
{
    public Guid UserId { get; set; }

    public DeleteUserCommand(Guid userId)
    {
        UserId = userId;
    }
}