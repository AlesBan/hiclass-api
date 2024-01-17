using HiClass.Application.Interfaces;
using HiClass.Domain.Entities.Location;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HiClass.Application.Handlers.EntityHandlers.CityHandlers.Queries.GetCity;

public class GetCityQueryHandler : IRequestHandler<GetCityQuery, City>
{
    private readonly ISharedLessonDbContext _context;

    public GetCityQueryHandler(ISharedLessonDbContext context)
    {
        _context = context;
    }

    public async Task<City> Handle(GetCityQuery request, CancellationToken cancellationToken)
    {
        var city = await _context.Cities.FirstOrDefaultAsync(c =>
            c.Country.CountryId == request.CountryId &&
            c.Title == request.Title, cancellationToken: cancellationToken);

        if (city != null)
        {
            return city;
        }

        var newCity = new City()
        {
            CountryId = request.CountryId,
            Title = request.Title
        };

        await _context.Cities.AddAsync(newCity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return newCity;
    }
}