using HiClass.Application.Interfaces;
using HiClass.Domain.Entities.Main;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HiClass.Application.Handlers.EntityHandlers.UserHandlers.Queries.GetUserProfileListByDefaultSearchRequest;

public class GetUserListByDefaultSearchRequestCommandHandler : IRequestHandler<
    GetUserListByDefaultSearchRequestCommand, IEnumerable<User>>
{
    private readonly ISharedLessonDbContext _context;

    public GetUserListByDefaultSearchRequestCommandHandler(ISharedLessonDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<User>> Handle(GetUserListByDefaultSearchRequestCommand request,
        CancellationToken cancellationToken)
    {
        var countryId = request.CountryId;
        var disciplineIds = request.DisciplineIds.ToList();
        var userIdToExclude = request.UserId;

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
            .Include(u => u.ReceivedInvitations).ThenInclude(ri => ri.Feedbacks)
            .Include(u => u.SentInvitations).ThenInclude(ri => ri.Feedbacks)
            .Where(u => u.UserId != userIdToExclude);

        // Создаем предикаты для условий страны и дисциплин
        var countryPredicate = query.Where(u =>
            u.CountryId == countryId);
        var disciplinePredicate = query.Where(u => u.UserDisciplines
            .Any(ud =>
                disciplineIds.Contains(ud.Discipline.DisciplineId)));

        // Объединяем предикаты через OR
        query = query.Where(u => countryPredicate.Any() || disciplinePredicate.Any());

        var users = await query.Take(100).ToListAsync(cancellationToken);

        return users;
    }
}