using HiClass.Application.Common.Exceptions.Database;
using HiClass.Application.Interfaces;
using HiClass.Domain.Entities.Location;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HiClass.Application.Handlers.EntityHandlers.CityHandlers.Commands.CreateCity;

public class CreateCityCommandHandler : IRequestHandler<CreateCityCommand, City>
{
    private readonly ISharedLessonDbContext _context;

    public CreateCityCommandHandler(ISharedLessonDbContext context)
    {
        _context = context;
    }

    public async Task<City> Handle(CreateCityCommand request, CancellationToken cancellationToken)
    {
        var country = await _context.Countries
            .FirstOrDefaultAsync(t =>
                    t.CountryId == request.CountryId,
                cancellationToken);

        if (country == null)
        {
            throw new NotFoundException(nameof(Country), request.CountryId);
        }

        var city = new City
        {
            Title = request.Title,
            CountryId = country.CountryId
        };
        await _context.SaveChangesAsync(cancellationToken);

        await AddCity(city, cancellationToken);

        return city;
    }

    private async Task AddCity(City city, CancellationToken cancellationToken)
    {
        await _context.Cities
            .AddAsync(city, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }
}