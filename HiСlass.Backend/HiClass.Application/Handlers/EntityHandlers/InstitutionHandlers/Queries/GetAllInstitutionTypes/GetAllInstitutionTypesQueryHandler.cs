using HiClass.Application.Interfaces;
using MediatR;

namespace HiClass.Application.Handlers.EntityHandlers.InstitutionHandlers.Queries.GetAllInstitutionTypes;

public class GetAllInstitutionTypesQueryHandler : IRequestHandler<GetAllInstitutionTypesQuery, List<string>>
{
    private readonly ISharedLessonDbContext _context;

    public GetAllInstitutionTypesQueryHandler(ISharedLessonDbContext context)
    {
        _context = context;
    }

    public Task<List<string>> Handle(GetAllInstitutionTypesQuery request, CancellationToken cancellationToken)
    {
        var titles = _context.InstitutionTypes.Select(i => i.Title).ToList();

        return Task.FromResult(titles);
    }
}