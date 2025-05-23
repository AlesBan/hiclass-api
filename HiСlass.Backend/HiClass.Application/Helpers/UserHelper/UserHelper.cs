using System.Security.Cryptography;
using HiClass.Application.Common.Exceptions.User;
using HiClass.Application.Common.Exceptions.User.Forbidden;
using HiClass.Application.Common.Exceptions.User.ResettingPassword;
using HiClass.Application.Handlers.EntityHandlers.UserHandlers.Queries.GetBlankUserById;
using HiClass.Application.Handlers.EntityHandlers.UserHandlers.Queries.GetBlankUserWithDevicesById;
using HiClass.Application.Handlers.EntityHandlers.UserHandlers.Queries.GetFullUserById;
using HiClass.Application.Handlers.EntityHandlers.UserHandlers.Queries.GetUserByClass;
using HiClass.Application.Handlers.EntityHandlers.UserHandlers.Queries.GetUserByEmail;
using HiClass.Application.Interfaces;
using HiClass.Domain.Entities.Main;
using MediatR;

namespace HiClass.Application.Helpers.UserHelper;

public class UserHelper : IUserHelper
{
    private readonly ISharedLessonDbContext _context;

    public UserHelper(ISharedLessonDbContext context)
    {
        _context = context;
    }

    public async Task UpdateAsync(User user)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync(CancellationToken.None);
    }

    public async Task<User> GetBlankUserById(Guid userId, IMediator mediator)
    {
        var user = await mediator.Send(new GetBlankUserByIdQuery(userId));

        return user;
    }

    public async Task<User> GetBlankUserWithDevicesById(Guid userId, IMediator mediator)
    {
        var command = new GetBlankUserWithDevicesByIdQuery(userId);

        var user = await mediator.Send(command);

        return user;
    }

    public async Task<User> GetFullUserById(Guid userId, IMediator mediator)
    {
        var user = await mediator.Send(new GetFullUserByIdQuery(userId));

        return user;
    }

    public async Task<User> GetBlankUserByEmail(string email, IMediator mediator)
    {
        try
        {
            var user = await mediator.Send(new GetBlankUserByEmailQuery(email));
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
        return GenerateCode();
    }

    public string GeneratePasswordResetCode()
    {
        return GenerateCode();
    }

    public void CheckResetPasswordCode(User user, string code)
    {
        if (user.PasswordResetCode != code)
        {
            throw new InvalidResetPasswordCodeException(user.UserId, code);
        }
    }

    private static string GenerateCode(int length = 6)
    {
        const string chars = "0123456789";
        var code = new char[length];

        for (var i = 0; i < length; i++)
        {
            var randomNumber = RandomNumberGenerator.GetInt32(0, chars.Length);
            code[i] = chars[randomNumber];
        }

        return new string(code);
    }
}