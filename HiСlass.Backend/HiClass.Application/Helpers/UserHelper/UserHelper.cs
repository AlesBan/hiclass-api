using System.Text;
using AutoMapper;
using HiClass.Application.Common.Exceptions.Database;
using HiClass.Application.Common.Exceptions.User;
using HiClass.Application.Common.Exceptions.User.Forbidden;
using HiClass.Application.Common.Exceptions.User.ResettingPassword;
using HiClass.Application.Handlers.EntityHandlers.UserHandlers.Queries.GetUserByClass;
using HiClass.Application.Handlers.EntityHandlers.UserHandlers.Queries.GetUserByEmail;
using HiClass.Application.Handlers.EntityHandlers.UserHandlers.Queries.GetUserById;
using HiClass.Application.Models.Class;
using HiClass.Application.Models.Institution;
using HiClass.Application.Models.Invitations.Feedbacks;
using HiClass.Application.Models.Invitations.Invitations;
using HiClass.Application.Models.User;
using HiClass.Domain.Entities.Communication;
using HiClass.Domain.Entities.Main;
using MediatR;

namespace HiClass.Application.Helpers.UserHelper;

public class UserHelper : IUserHelper
{

    public async Task<User> GetUserById(Guid userId, IMediator mediator)
    {
        var user = await mediator.Send(new GetUserByIdQuery(userId));

        return user;
    }

    public async Task<User> GetUserByEmail(string email, IMediator mediator)
    {
        try
        {
            var user = await mediator.Send(new GetUserByEmailQuery(email));
            return user;
        }
        catch (UserNotFoundByEmailException)
        {
            throw new InvalidInputCredentialsException("User with this email does not exist");
        }
    }

    public async Task<Guid> GetUserIdByClassId(Guid classId, IMediator mediator)
    {
        var command = new GetUserIdByClassIdQuery(classId);

        var userId = await mediator.Send(command);

        return userId;
    }

    public void CheckUserVerification(User user)
    {
        if (!user.IsVerified)
        {
            throw new UserNotVerifiedException(user.UserId);
        }
    }

    public void CheckUserCreateAccountAbility(User user)
    {
        if (user.IsCreatedAccount)
        {
            throw new UserAlreadyHasAccountException(user.UserId);
        }
    }

    public string GenerateVerificationCode()
    {
        var random = new Random();
        var verificationCodeBuilder = new StringBuilder();

        for (var i = 0; i < 6; i++)
        {
            verificationCodeBuilder.Append(random.Next(0, 10));
        }

        return verificationCodeBuilder.ToString();
    }

    public void CheckResetTokenValidation(User user)
    {
        if (user.PasswordResetToken == null)
        {
            throw new InvalidResetTokenProvidedException();
        }

        if (user.ResetTokenExpires < DateTime.Now)
        {
            throw new ResetTokenHasExpiredException(user.UserId, user.PasswordResetToken ?? "");
        }
    }

    public string GeneratePasswordResetCode()
    {
        var random = new Random();
        var verificationCodeBuilder = new StringBuilder();

        for (var i = 0; i < 6; i++)
        {
            verificationCodeBuilder.Append(random.Next(0, 10));
        }

        return verificationCodeBuilder.ToString();
    }

    public void CheckResetPasswordCode(User user, string code)
    {
        if (user.PasswordResetCode != code)
        {
            throw new InvalidResetPasswordCodeException(user.UserId, code);
        }
    }
}