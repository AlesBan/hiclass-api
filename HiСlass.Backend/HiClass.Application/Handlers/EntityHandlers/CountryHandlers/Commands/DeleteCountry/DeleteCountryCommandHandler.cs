using HiClass.Application.Common.Exceptions.Database;
using HiClass.Application.Interfaces;
using HiClass.Domain.Entities.Location;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HiClass.Application.Handlers.EntityHandlers.CountryHandlers.Commands.DeleteCountry;

public class DeleteCountryCommandHandler : IRequestHandler<DeleteCountryCommand, Unit>
{
    private readonly ISharedLessonDbContext _context;

    public DeleteCountryCommandHandler(ISharedLessonDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteCountryCommand request, CancellationToken cancellationToken)
    {
        var country = await _context.Countries!.FirstOrDefaultAsync(t =>
                t.CountryId == request.CountryId,
            cancellationToken: cancellationToken);

        if (country == null)
        {
            throw new NotFoundException(nameof(Country), request.CountryId);
        }

        await RemoveCountry(country, cancellationToken);

        return Unit.Value;
    }

    private async Task RemoveCountry(Country country, CancellationToken cancellationToken)
    {
        _context.Countries!.Remove(country);
        await _context.SaveChangesAsync(cancellationToken);
    }
}