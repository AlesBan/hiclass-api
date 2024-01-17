using HiClass.Domain.Entities.Main;
using MediatR;

namespace HiClass.Application.Handlers.EntityHandlers.UserHandlers.Commands.UpdateUserToken;

public class UpdateUserAccessTokenCommand : IRequest<User>
{
    public Guid UserId { get; set; }
    public string VerificationToken { get; set; }

    public UpdateUserAccessTokenCommand(Guid userId, string verificationToken)
    {
        UserId = userId;
        VerificationToken = verificationToken;
    }
}