using HiClass.Application.Common.Exceptions.User;
using HiClass.Application.Helpers.UserHelper;
using HiClass.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HiClass.Application.Handlers.EntityHandlers.UserHandlers.Commands.UpdateUserVerificationCode;

public class UpdateUserVerificationCodeCommandHandler : IRequestHandler<UpdateUserVerificationCodeCommand, string>
{
    private readonly ISharedLessonDbContext _context;
    private readonly IUserHelper _userHelper;

    public UpdateUserVerificationCodeCommandHandler(ISharedLessonDbContext context, IUserHelper userHelper)
    {
        _context = context;
        _userHelper = userHelper;
    }

    public async Task<string> Handle(UpdateUserVerificationCodeCommand request, CancellationToken cancellationToken)
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
                u.Email == request.Email, cancellationToken: cancellationToken);

        if (user == null)
            throw new UserNotFoundByEmailException(request.Email);

        user.VerificationCode = _userHelper.GenerateVerificationCode();

        _context.Users.Update(user);

        await _context.SaveChangesAsync(cancellationToken);

        return user.VerificationCode;
    }
}