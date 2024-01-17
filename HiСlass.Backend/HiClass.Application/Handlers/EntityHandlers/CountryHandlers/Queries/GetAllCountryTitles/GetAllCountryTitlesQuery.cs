using MediatR;

namespace HiClass.Application.Handlers.EntityHandlers.CountryHandlers.Queries.GetAllCountryTitles;

public class GetAllCountryTitlesQuery : IRequest<List<string>>
{
}