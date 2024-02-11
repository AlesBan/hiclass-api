using HiClass.Application.Common.Exceptions.Database;
using HiClass.Application.Common.Exceptions.User;
using HiClass.Application.Helpers.TokenHelper;
using HiClass.Application.Interfaces;
using HiClass.Domain.Entities.Main;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HiClass.Application.Handlers.EntityHandlers.UserHandlers.Commands.UpdateUserVerification;

public class UpdateUserVerificationCommandHandler : IRequestHandler<UpdateUserVerificationCommand, User>
{
    private readonly ISharedLessonDbContext _context;
    private readonly ITokenHelper _tokenHelper;

    public UpdateUserVerificationCommandHandler(ISharedLessonDbContext sharedLessonDbContext, ITokenHelper tokenHelper)
    {
        _context = sharedLessonDbContext;
        _tokenHelper = tokenHelper;
    }

    public async Task<User> Handle(UpdateUserVerificationCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(x =>
                x.UserId == request.UserId, cancellationToken);

        if (user == null)
        {
            throw new UserNotFoundException(request.UserId);
        }
        
        if (user.VerificationCode != request.VerificationCode)
        {
            throw new InvalidVerificationCodeProvidedException();
        }

        user.IsVerified = true;
        user.VerifiedAt = DateTime.UtcNow;
        user.AccessToken = request.AccessToken;
        
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