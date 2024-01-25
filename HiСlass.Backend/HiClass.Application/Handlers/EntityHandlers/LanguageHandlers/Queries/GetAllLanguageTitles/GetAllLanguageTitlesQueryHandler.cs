using HiClass.Application.Interfaces;
using MediatR;

namespace HiClass.Application.Handlers.EntityHandlers.LanguageHandlers.Queries.GetAllLanguageTitles;

public class GetAllLanguageTitlesQueryHandler: IRequestHandler<GetAllLanguageTitlesQuery, List<string>>
{
    private readonly ISharedLessonDbContext _context;

    public GetAllLanguageTitlesQueryHandler(ISharedLessonDbContext context)
    {
        _context = context;
    }

    public Task<List<string>> Handle(GetAllLanguageTitlesQuery request, CancellationToken cancellationToken)
    {
        var titles = _context.Languages.Select(l => l.Title).ToList();
        
        return Task.FromResult(titles);
    }
}