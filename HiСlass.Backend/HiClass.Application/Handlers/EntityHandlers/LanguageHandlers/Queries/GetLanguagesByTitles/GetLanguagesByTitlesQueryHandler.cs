using HiClass.Application.Common.Exceptions.Database;
using HiClass.Application.Interfaces;
using HiClass.Domain.Entities.Education;
using MediatR;

namespace HiClass.Application.Handlers.EntityHandlers.LanguageHandlers.Queries.GetLanguagesByTitles;

public class GetLanguagesByTitlesQueryHandler : IRequestHandler<GetLanguagesByTitlesQuery, List<Language>>
{
    private readonly ISharedLessonDbContext _context;

    public GetLanguagesByTitlesQueryHandler(ISharedLessonDbContext context)
    {
        _context = context;
    }

    public Task<List<Language>> Handle(GetLanguagesByTitlesQuery request, CancellationToken cancellationToken)
    {
        var requestLanguageTitles = request.LanguageTitles;
        var languages = _context.Languages
            .Where(l => requestLanguageTitles.Contains(l.Title))
            .ToList();

        foreach (var languageTitle in requestLanguageTitles.Where(language =>
                     !languages.Select(l => l.Title).Contains(language)))
        {
            throw new NotFoundException(nameof(Language), languageTitle);
        }

        return Task.FromResult(languages.ToList());
    }
}