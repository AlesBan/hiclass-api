using HiClass.Domain.Entities.Education;
using MediatR;

namespace HiClass.Application.Handlers.EntityHandlers.LanguageHandlers.Queries.GetLanguagesByTitles;

public class GetLanguagesByTitlesQuery : IRequest<List<Language>>
{
    public IEnumerable<string> LanguageTitles { get; set; }

    public GetLanguagesByTitlesQuery(IEnumerable<string> languageTitles)
    {
        LanguageTitles = languageTitles;
    }
}