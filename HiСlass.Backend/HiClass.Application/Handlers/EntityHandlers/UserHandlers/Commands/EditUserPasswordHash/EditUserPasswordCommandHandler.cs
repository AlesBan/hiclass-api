using HiClass.Application.Common.Exceptions.User;
using HiClass.Application.Helpers;
using HiClass.Application.Interfaces;
using HiClass.Application.Models.User.Authentication;
using HiClass.Domain.Entities.Main;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HiClass.Application.Handlers.EntityHandlers.UserHandlers.Commands.EditUserPasswordHash;

public class EditUserPasswordCommandHandler : IRequestHandler<EditUserPasswordCommand, User>
{
    private readonly ISharedLessonDbContext _context;

    public EditUserPasswordCommandHandler(ISharedLessonDbContext context)
    {
        _context = context;
    }

    public async Task<User> Handle(EditUserPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = _context.Users
            .AsNoTracking()
            .FirstOrDefault(u =>
                u.UserId == request.UserId);

        if (user == null)
        {
            throw new UserNotFoundByIdException(request.UserId);
        }
        
        if (!user.IsPasswordSet)
        {
            throw new UserPasswordNotSetException(request.UserId);
        }

        PasswordHelper.VerifyPasswordHash(user, request.OldPassword);
        PasswordHelper.CreatePasswordHash(request.NewPassword, out var passwordHash, out var passwordSalt);
        user.PasswordHash = passwordHash;
        user.PasswordSalt = passwordSalt;

        _context.Users.Update(user);
        await _context.SaveChangesAsync(cancellationToken);
        
        user = _context.Users
            .Include(u => u.City)
            .Include(u => u.Country)
            .Include(u => u.Institution)
            .Include(u => u.Classes)
            .ThenInclude(c => c.ClassLanguages)
            .ThenInclude(cl => cl.Language)
            .Include(u => u.Classes)
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