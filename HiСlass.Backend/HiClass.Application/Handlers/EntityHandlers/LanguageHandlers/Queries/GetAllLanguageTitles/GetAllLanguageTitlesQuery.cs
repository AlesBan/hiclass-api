using MediatR;

namespace HiClass.Application.Handlers.EntityHandlers.LanguageHandlers.Queries.GetAllLanguageTitles;

public class GetAllLanguageTitlesQuery : IRequest<List<string>>
{
}