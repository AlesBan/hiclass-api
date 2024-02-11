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
    private readonly IUserHelper _userHelper;

    public UpdateUserResetPasswordInfoCommandHandler(ISharedLessonDbContext context, IUserHelper userHelper)
    {
        _context = context;
        _userHelper = userHelper;
    }

    public async Task<User> Handle(UpdateUserResetPasswordInfoCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(x => x.UserId == request.UserId, cancellationToken: cancellationToken);

        if (user == null)
            throw new UserNotFoundException(request.UserId);

        user.PasswordResetToken = request.AccessToken;
        user.ResetTokenExpires = DateTime.UtcNow.AddHours(4);
        
        user.PasswordResetCode = _userHelper.GeneratePasswordResetCode();
        
        _context.Users.Attach(user).State = EntityState.Modified;

        await _context.SaveChangesAsync(cancellationToken);
        
        user = _context.Users
            .Include(u => u.City)
            .Include(u => u.Country)
            .Include(u => u.Institution)
            .Include(u => u.Classes)
            .ThenInclude(c => c.ClassLanguages)
            .ThenInclude(cl => cl.Language)
            .Include(u => u.Classes)
            .ThenInclude(c => c.ClassDisciplines)
            .ThenInclude(cd => cd.Discipline)
            .Include(u => u.Classes)
            .ThenInclude(c => c.Grade)
            .Include(u => u.UserDisciplines)
            .ThenInclude(ud => ud.Discipline)
            .Include(u => u.UserLanguages)
            .ThenInclude(ul => ul.Language)
            .Include(u => u.UserGrades)
            .ThenInclude(ug => ug.Grade)
            .FirstOrDefault(u =>
                u.UserId == request.UserId);
        return user;
    }
}