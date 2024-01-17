using HiClass.Application.Common.Exceptions.Database;
using HiClass.Application.Helpers.TokenHelper;
using HiClass.Application.Helpers.UserHelper;
using HiClass.Application.Interfaces;
using HiClass.Domain.Entities.Main;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HiClass.Application.Handlers.EntityHandlers.UserHandlers.Commands.UpdateUserResetToken;

public class UpdateUserResetPasswordInfoCommandHandler : IRequestHandler<UpdateUserResetPasswordInfoCommand, User>
{
    private readonly ISharedLessonDbContext _context;
    private readonly ITokenHelper _tokenHelper;
    private readonly IUserHelper _userHelper;

    public UpdateUserResetPasswordInfoCommandHandler(ISharedLessonDbContext context, ITokenHelper tokenHelper, IUserHelper userHelper)
    {
        _context = context;
        _tokenHelper = tokenHelper;
        _userHelper = userHelper;
    }

    public async Task<User> Handle(UpdateUserResetPasswordInfoCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(x => x.UserId == request.UserId, cancellationToken: cancellationToken);

        if (user == null)
            throw new UserNotFoundException(request.UserId);

        user.PasswordResetToken = _tokenHelper.CreateToken(user);
        user.ResetTokenExpires = DateTime.UtcNow.AddHours(4);
        
        user.PasswordResetCode = _userHelper.GeneratePasswordResetCode();
        
        _context.Users.Attach(user).State = EntityState.Modified;

        await _context.SaveChangesAsync(cancellationToken);
        
        return user;
    }
}