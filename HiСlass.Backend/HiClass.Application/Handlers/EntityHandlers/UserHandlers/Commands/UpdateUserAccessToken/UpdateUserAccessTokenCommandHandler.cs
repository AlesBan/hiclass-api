using HiClass.Application.Common.Exceptions.Database;
using HiClass.Application.Common.Exceptions.User;
using HiClass.Application.Interfaces;
using HiClass.Domain.Entities.Main;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HiClass.Application.Handlers.EntityHandlers.UserHandlers.Commands.UpdateUserAccessToken;

public class UpdateUserAccessTokenCommandHandler : IRequestHandler<EditUserAccessTokenCommand, User>
{
    private readonly ISharedLessonDbContext _context;

    public UpdateUserAccessTokenCommandHandler(ISharedLessonDbContext context)
    {
        _context = context;
    }

    public async Task<User> Handle(EditUserAccessTokenCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users.AsNoTracking()
            .FirstOrDefaultAsync(u => u.UserId == request.UserId, cancellationToken);

        if (user == null)
            throw new UserNotFoundByIdException(request.UserId);

        user.AccessToken = request.AccessToken;
        
        _context.Users.Update(user);
        await _context.SaveChangesAsync(cancellationToken);

        var verifiedUser = _context
            .Users
            .Include(u => u.Country)
            .Include(u => u.City)
            .Include(u => u.Institution)
            .Include(u => u.UserDisciplines)
            .ThenInclude(ud => ud.Discipline)
            .Include(u => u.UserLanguages)
            .ThenInclude(ul => ul.Language)
            .Include(u => u.UserGrades)
            .ThenInclude(ug => ug.Grade)
            .FirstOrDefault(u => u.UserId == request.UserId);

        return verifiedUser!;
    }
}