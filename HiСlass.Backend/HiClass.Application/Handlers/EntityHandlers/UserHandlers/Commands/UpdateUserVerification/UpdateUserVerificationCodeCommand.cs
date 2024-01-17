using HiClass.Domain.Entities.Main;
using MediatR;

namespace HiClass.Application.Handlers.EntityHandlers.UserHandlers.Commands.UpdateUserVerification;

public class UpdateUserVerificationCodeCommand : IRequest<User>
{
    public Guid UserId { get; set; }
    public string VerificationCode { get; set; }

    public UpdateUserVerificationCodeCommand(Guid userId, string verificationCode)
    {
        UserId = userId;
        VerificationCode = verificationCode;
    }
}