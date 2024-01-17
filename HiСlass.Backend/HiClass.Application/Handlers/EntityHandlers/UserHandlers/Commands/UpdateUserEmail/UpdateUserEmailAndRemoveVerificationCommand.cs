using HiClass.Domain.Entities.Main;
using MediatR;

namespace HiClass.Application.Handlers.EntityHandlers.UserHandlers.Commands.UpdateUserEmail;

public class UpdateUserEmailAndRemoveVerificationCommand : IRequest<User>
{
    public Guid UserId { get; set; }
    public string Email { get; set; }

    public UpdateUserEmailAndRemoveVerificationCommand(Guid userId, string email)
    {
        UserId = userId;
        Email = email;
    }
}