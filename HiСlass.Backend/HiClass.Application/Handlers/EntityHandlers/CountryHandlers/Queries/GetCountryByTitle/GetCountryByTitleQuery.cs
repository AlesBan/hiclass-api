using HiClass.Domain.Entities.Location;
using MediatR;

namespace HiClass.Application.Handlers.EntityHandlers.CountryHandlers.Queries.GetCountryByTitle;

public class GetCountryByTitleQuery : IRequest<Country>
{
    public string Title { get; set; }

    public GetCountryByTitleQuery(string title) 
    {
        Title = title;
    }
}