using HiClass.Application.Interfaces;
using HiClass.Domain.Entities.Location;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HiClass.Application.Handlers.EntityHandlers.CountryHandlers.Queries.GetCountryByTitle;

public class GetCountryByTitleQueryHandler : IRequestHandler<GetCountryByTitleQuery, Country>
{
    private readonly ISharedLessonDbContext _context;

    public GetCountryByTitleQueryHandler(ISharedLessonDbContext context)
    {
        _context = context;
    }

    public async Task<Country> Handle(GetCountryByTitleQuery request, CancellationToken cancellationToken)
    {
        var country = await _context.Countries
            .FirstOrDefaultAsync(c =>
                c.Title == request.Title, cancellationToken: cancellationToken);

        if (country != null)
        {
            return country;
        }

        var newCountry = new Country()
        {
            Title = request.Title
        };

        await _context.Countries.AddAsync(newCountry, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        
        return newCountry;
    }
}