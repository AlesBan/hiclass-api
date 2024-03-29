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
        var disciplineIds = searchRequest.DisciplineIds;
        var userIdToExclude = searchRequest.UserId;

        var users = await _context.Users
            .Include(u => u.City)
            .Include(u => u.Country)
            .Include(u => u.Institution)
            .Include(u => u.Classes).ThenInclude(c => c.ClassLanguages).ThenInclude(cl => cl.Language)
            .Include(u => u.Classes).ThenInclude(c => c.ClassDisciplines).ThenInclude(cd => cd.Discipline)
            .Include(u => u.Classes).ThenInclude(cg => cg.Grade)
            .Include(u => u.UserDisciplines).ThenInclude(ud => ud.Discipline)
            .Include(u => u.UserLanguages).ThenInclude(ul => ul.Language)
            .Include(u => u.UserGrades).ThenInclude(ug => ug.Grade)
            .Where(u =>
                u.UserId != userIdToExclude && (
                    u.Country != null && u.CountryId == countryId ||
                    u.UserDisciplines.Any(ud =>
                        disciplineIds.Contains(ud.Discipline.DisciplineId))))
            .ToListAsync(cancellationToken: cancellationToken);

        return users;
    }
}