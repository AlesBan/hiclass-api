using HiClass.Application.Common.Exceptions.Database;
using HiClass.Application.Common.Exceptions.User;
using HiClass.Application.Interfaces;
using HiClass.Domain.Entities.Main;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HiClass.Application.Handlers.EntityHandlers.UserHandlers.Queries.GetUserById;

public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, User>
{
    private readonly ISharedLessonDbContext _context;

    public GetUserByIdQueryHandler(ISharedLessonDbContext context)
    {
        _context = context;
    }

    public async Task<User> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .Include(u => u.Country)
            .Include(u => u.City)
            .Include(u => u.Institution)
            .FirstOrDefaultAsync(u =>
                u.UserId == request.UserId, cancellationToken: cancellationToken);

        if (user == null)
        {
            throw new UserNotFoundByIdException(request.UserId);
        }

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
            .ThenInclude(cg => cg.Grade)
            .Include(u => u.UserDisciplines)
            .ThenInclude(ud => ud.Discipline)
            .Include(u => u.UserLanguages)
            .ThenInclude(ul => ul.Language)
            .Include(u => u.UserGrades)
            .ThenInclude(ug => ug.Grade)
            .Include(u => u.ReceivedFeedbacks)
            .ThenInclude(rf => rf.UserSender)
            .ThenInclude(rf => rf.Country)
            .Include(u => u.ReceivedFeedbacks)
            .ThenInclude(rf => rf.UserSender)
            .ThenInclude(rf => rf.City)
            .Include(u => u.UserGrades).ThenInclude(ug => ug.Grade)
            .Include(u => u.ReceivedInvitations)
            .ThenInclude(ri => ri.Feedbacks)
            .ThenInclude(f => f.UserSender)
            .Include(u => u.SentInvitations)
            .ThenInclude(ri => ri.Feedbacks)
            .ThenInclude(f => f.UserSender)
            .FirstOrDefault(u =>
                u.UserId == request.UserId);

        return user;
    }
}