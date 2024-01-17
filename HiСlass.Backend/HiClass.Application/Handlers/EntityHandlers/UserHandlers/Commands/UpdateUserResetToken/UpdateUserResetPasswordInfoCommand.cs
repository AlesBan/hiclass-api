using HiClass.Domain.Entities.Main;
using MediatR;

namespace HiClass.Application.Handlers.EntityHandlers.UserHandlers.Commands.UpdateUserResetToken;

public class UpdateUserResetPasswordInfoCommand : IRequest<User>
{
    public Guid UserId { get; set; }

    public UpdateUserResetPasswordInfoCommand(Guid userId)
    {
        UserId = userId;
    }
}