using AutoMapper;
using HiClass.Application.Interfaces;
using HiClass.Domain.Entities.Main;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HiClass.Application.Handlers.EntityHandlers.UserHandlers.Queries.GetUserProfileListByDefaultSearchRequest;

public class GetUserListByDefaultSearchRequestCommandHandler : IRequestHandler<
    GetUserListByDefaultSearchRequestCommand, IEnumerable<User>>
{
    private readonly ISharedLessonDbContext _context;
    private readonly IMapper _mapper;

    public GetUserListByDefaultSearchRequestCommandHandler(ISharedLessonDbContext context, IMapper mapper)
    {
        _mapper = mapper;
        _context = context;
    }

    public async Task<IEnumerable<User>> Handle(GetUserListByDefaultSearchRequestCommand request,
        CancellationToken cancellationToken)
    {
        var searchRequest = request.SearchRequest;
        var countryId = searchRequest.CountryId;
        var disciplineIds = searchRequest.DisciplineIds.ToList();
        var userIdToExclude = searchRequest.UserId;

        var query = _context.Users
            .Include(u => u.City)
            .Include(u => u.Country)
            .Include(u => u.Institution)
            .Include(u => u.Classes).ThenInclude(c => c.ClassLanguages).ThenInclude(cl => cl.Language)
            .Include(u => u.Classes).ThenInclude(c => c.ClassDisciplines).ThenInclude(cd => cd.Discipline)
            .Include(u => u.Classes).ThenInclude(cg => cg.Grade)
            .Include(u => u.UserDisciplines).ThenInclude(ud => ud.Discipline)
            .Include(u => u.UserLanguages).ThenInclude(ul => ul.Language)
            .Include(u => u.UserGrades).ThenInclude(ug => ug.Grade)
            .Include(u => u.ReceivedInvitations)
            .ThenInclude(ri => ri.Feedbacks)
            .Include(u => u.SentInvitations)
            .ThenInclude(ri => ri.Feedbacks)
            .Where(u => u.UserId != userIdToExclude);


        if (!string.IsNullOrEmpty(countryId.ToString()))
        {
            query = query.Where(u => u.CountryId == countryId);
        }

        if (disciplineIds.Any())
        {
            query = query.Where(u =>
                u.UserDisciplines.Any(ud =>
                    disciplineIds.Contains(ud.Discipline.DisciplineId)));
        }

        var users = await query.Take(100).ToListAsync(cancellationToken);

        return users;
    }
}