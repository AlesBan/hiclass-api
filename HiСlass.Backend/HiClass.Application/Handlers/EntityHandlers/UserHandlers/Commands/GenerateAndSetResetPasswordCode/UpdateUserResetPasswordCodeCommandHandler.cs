using HiClass.Application.Common.Exceptions.User;
using HiClass.Application.Helpers.UserHelper;
using HiClass.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HiClass.Application.Handlers.EntityHandlers.UserHandlers.Commands.GenerateAndSetResetPasswordCode;

public class UpdateUserResetPasswordCodeCommandHandler : IRequestHandler<UpdateUserResetPasswordCodeCommand, string>
{
    private readonly ISharedLessonDbContext _context;

    private readonly IUserHelper _userHelper;

    public UpdateUserResetPasswordCodeCommandHandler(ISharedLessonDbContext context, IUserHelper userHelper)
    {
        _context = context;
        _userHelper = userHelper;
    }

    public async Task<string> Handle(UpdateUserResetPasswordCodeCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Email == request.Email, cancellationToken);
        
        if (user == null)
            throw new UserNotFoundByEmailException(request.Email);
        
        user.PasswordResetCode = _userHelper.GeneratePasswordResetCode();

        _context.Users.Update(user);
        await _context.SaveChangesAsync(CancellationToken.None);
        
        return user.PasswordResetCode;
    }
}