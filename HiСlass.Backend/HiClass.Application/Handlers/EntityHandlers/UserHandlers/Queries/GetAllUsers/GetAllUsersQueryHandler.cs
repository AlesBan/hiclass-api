using HiClass.Application.Interfaces;
using HiClass.Domain.Entities.Main;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HiClass.Application.Handlers.EntityHandlers.UserHandlers.Queries.GetAllUsers;

public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, List<User>>
{
    private readonly ISharedLessonDbContext _sharedLessonDbContext;

    public GetAllUsersQueryHandler(ISharedLessonDbContext sharedLessonDbContext)
    {
        _sharedLessonDbContext = sharedLessonDbContext;
    }

    public Task<List<User>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        var users = _sharedLessonDbContext.Users
            .Include(u => u.City)
            .Include(u => u.Country)
            .Include(u => u.Institution)
            .Include(u => u.Classes)
            .ThenInclude(c => c.ClassLanguages)
            .ThenInclude(cl => cl.Language)
            .Include(u => u.Classes)
            .ThenInclude(cd => cd.Discipline)
            .Include(u => u.Classes)
            .ThenInclude(cg => cg.Grade)
            .Include(u => u.UserDisciplines)
            .ThenInclude(ud => ud.Discipline)
            .Include(u => u.UserLanguages)
            .ThenInclude(ul => ul.Language)
            .Include(u => u.UserGrades)
            .ThenInclude(ug => ug.Grade)
            .Include(u => u.ReceivedInvitations)
            .ThenInclude(ri => ri.Feedbacks)
            .ThenInclude(rf => rf.UserFeedbackSender)
            .ThenInclude(us => us.City)
            .ThenInclude(c => c!.Country)
            .Include(u => u.ReceivedFeedbacks)
            .ThenInclude(rf => rf.UserFeedbackSender)
            .ThenInclude(us => us.City)
            .ThenInclude(c => c!.Country)
            .Include(u => u.UserDevices)
            .ThenInclude(ud => ud.Device)
            .ToListAsync(cancellationToken)
            .Result;

        return Task.FromResult(users);
    }
}