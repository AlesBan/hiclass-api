using HiClass.Application.Interfaces;
using HiClass.Domain.Entities.Main;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HiClass.Application.Handlers.EntityHandlers.UserHandlers.Queries.GetUserProfileListBySearchRequest;

public class GetUserListBySearchRequestCommandHandler : IRequestHandler<
    GetUserListBySearchRequestCommand, IEnumerable<User>>
{
    private readonly ISharedLessonDbContext _context;

    public GetUserListBySearchRequestCommandHandler(ISharedLessonDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<User>> Handle(GetUserListBySearchRequestCommand request, CancellationToken cancellationToken)
    {
        var searchRequest = request.SearchRequest;
        var countryTitles = searchRequest.Countries.ToList();
        var gradeNumbers = searchRequest.Grades.ToList();
        var disciplineTitles = searchRequest.Disciplines.ToList();
        var languageTitles = searchRequest.Languages.ToList();
        var userIdToExclude = searchRequest.UserId;

        var query = _context.Users
            .Include(u => u.City)
            .Include(u => u.Country)
            .Include(u => u.Institution)
            .Include(u => u.Classes).ThenInclude(c => c.ClassLanguages).ThenInclude(cl => cl.Language)
            .Include(u => u.Classes).ThenInclude(cd => cd.Discipline)
            .Include(u => u.Classes).ThenInclude(cg => cg.Grade)
            .Include(u => u.UserDisciplines).ThenInclude(ud => ud.Discipline)
            .Include(u => u.UserLanguages).ThenInclude(ul => ul.Language)
            .Include(u => u.UserGrades).ThenInclude(ug => ug.Grade)
            .Include(u => u.ReceivedInvitations)
            .ThenInclude(ri => ri.Feedbacks)
            .Include(u => u.SentInvitations)
            .ThenInclude(ri => ri.Feedbacks)
            .Where(u => u.UserId != userIdToExclude);

        if (countryTitles.Any())
        {
            query = query.Where(u => u.Country != null && countryTitles.Contains(u.Country.Title));
        }

        if (gradeNumbers.Any())
        {
            query = query.Where(u => u.UserGrades.Any(ug => gradeNumbers.Contains(ug.Grade.GradeNumber)));
        }

        if (disciplineTitles.Any())
        {
            query = query.Where(u => u.UserDisciplines.Any(d => disciplineTitles.Contains(d.Discipline.Title)));
        }

        if (languageTitles.Any())
        {
            query = query.Where(u => u.UserLanguages.Any(l => languageTitles.Contains(l.Language.Title)));
        }

        var users = await query.Take(100).ToListAsync(cancellationToken);

        return users;
    }

}