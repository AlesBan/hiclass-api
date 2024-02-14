using HiClass.Application.Common.Exceptions.User;
using HiClass.Application.Helpers;
using HiClass.Application.Helpers.TokenHelper;
using HiClass.Application.Helpers.UserHelper;
using HiClass.Application.Interfaces;
using HiClass.Domain.Entities.Main;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HiClass.Application.Handlers.EntityHandlers.UserHandlers.Commands.RegisterUser;

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, User>
{
    private readonly ISharedLessonDbContext _context;
    private readonly ITokenHelper _tokenHelper;
    private readonly IUserHelper _userHelper;

    public RegisterUserCommandHandler(ISharedLessonDbContext context, ITokenHelper tokenHelper, IUserHelper userHelper)
    {
        _context = context;
        _tokenHelper = tokenHelper;
        _userHelper = userHelper;
    }

    public async Task<User> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var userEmail = request.Email;
        var userPassword = request.Password;

        var userExists = await _context.Users.AsNoTracking()
            .AnyAsync(x => x.Email == userEmail, cancellationToken);

        if (userExists)
        {
            throw new UserAlreadyExistsException(userEmail);
        }

        var newUser = new User
        {
            Email = userEmail
        };

        PasswordHelper.SetUserPasswordHash(newUser, userPassword);

        newUser.VerificationCode = request.VerificationCode;

        await AddUserToDataBase(newUser, cancellationToken);

        return await Task.FromResult(newUser);
    }

    private async Task AddUserToDataBase(User user, CancellationToken cancellationToken)
    {
        await _context.Users
            .AddAsync(user, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }
}