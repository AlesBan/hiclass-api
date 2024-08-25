using HiClass.Application.Common.Exceptions.User;
using HiClass.Application.Interfaces;
using HiClass.Domain.Entities.Main;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HiClass.Application.Handlers.EntityHandlers.UserHandlers.Commands.EditUserEmail;

public class EditUserEmailAndRemoveVerificationCommandHandler :
    IRequestHandler<EditUserEmailAndRemoveVerificationCommand, User>
{
    private readonly ISharedLessonDbContext _context;

    public EditUserEmailAndRemoveVerificationCommandHandler(ISharedLessonDbContext context)
    {
        _context = context;
    }

    public async Task<User> Handle(EditUserEmailAndRemoveVerificationCommand request,
        CancellationToken cancellationToken)
    {
        var user = await _context.Users
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
            .FirstOrDefaultAsync(u =>
                u.UserId == request.UserId, cancellationToken: cancellationToken);

        if (user == null)
        {
            throw new UserNotFoundByIdException(request.UserId);
        }

        user.Email = request.Email;
        user.IsVerified = false;

        _context.Users.Update(user);

        await _context.SaveChangesAsync(cancellationToken);

        return user;
    }
}