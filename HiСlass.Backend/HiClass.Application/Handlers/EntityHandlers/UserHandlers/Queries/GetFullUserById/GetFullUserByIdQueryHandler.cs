using HiClass.Application.Common.Exceptions.User;
using HiClass.Application.Interfaces;
using HiClass.Domain.Entities.Main;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HiClass.Application.Handlers.EntityHandlers.UserHandlers.Queries.GetFullUserById;

public class GetFullUserByIdQueryHandler : IRequestHandler<GetFullUserByIdQuery, User>
{
    private readonly ISharedLessonDbContext _context;

    public GetFullUserByIdQueryHandler(ISharedLessonDbContext context)
    {
        _context = context;
    }

    public async Task<User> Handle(GetFullUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u =>
                u.UserId == request.UserId, cancellationToken: cancellationToken);

        if (user == null)
        {
            throw new UserNotFoundByIdException(request.UserId);
        }

        user = _context.Users
            .AsNoTracking()
            .Include(u => u.City)
            .Include(u => u.Country)
            .Include(u => u.Institution)
            .AsNoTracking()
            .Include(u => u.Classes)
            .ThenInclude(c => c.ClassLanguages)
            .ThenInclude(cl => cl.Language)
            .AsNoTracking()
            .Include(u => u.Classes)
            .ThenInclude(cd => cd.Discipline)
            .AsNoTracking()
            .Include(u => u.Classes)
            .ThenInclude(cg => cg.Grade)
            .AsNoTracking()
            .Include(u => u.UserDisciplines)
            .ThenInclude(ud => ud.Discipline)
            .AsNoTracking()
            .Include(u => u.UserLanguages)
            .ThenInclude(ul => ul.Language)
            .AsNoTracking()
            .Include(u => u.UserGrades)
            .ThenInclude(ug => ug.Grade)
            .AsNoTracking()
            .Include(u => u.ReceivedFeedbacks)
            .ThenInclude(rf => rf.UserFeedbackSender)
            .ThenInclude(rf => rf.Country)
            .Include(u => u.ReceivedFeedbacks)
            .ThenInclude(rf => rf.UserFeedbackSender)
            .ThenInclude(rf => rf.City)
            .Include(u => u.UserGrades).ThenInclude(ug => ug.Grade)
            .Include(u => u.ReceivedInvitations)
            .ThenInclude(ri => ri.Feedbacks)
            .ThenInclude(f => f.UserFeedbackSender)
            .Include(u => u.SentInvitations)
            .ThenInclude(ri => ri.Feedbacks)
            .ThenInclude(f => f.UserFeedbackSender)
            .FirstOrDefault(u =>
                u.UserId == request.UserId);

        return user!;
    }
}