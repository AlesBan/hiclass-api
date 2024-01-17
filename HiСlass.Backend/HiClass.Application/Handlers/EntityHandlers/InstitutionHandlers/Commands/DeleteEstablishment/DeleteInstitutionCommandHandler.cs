using HiClass.Application.Common.Exceptions.Database;
using HiClass.Application.Interfaces;
using HiClass.Domain.Entities.Job;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HiClass.Application.Handlers.EntityHandlers.InstitutionHandlers.Commands.DeleteEstablishment;

public class DeleteInstitutionCommandHandler : IRequestHandler<DeleteInstitutionCommand, Unit>
{
    private readonly ISharedLessonDbContext _context;

    public DeleteInstitutionCommandHandler(ISharedLessonDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteInstitutionCommand request, CancellationToken cancellationToken)
    {
        var establishment = await _context.Institutions.FirstOrDefaultAsync(e =>
                e.InstitutionId == request.EstablishmentId,
            cancellationToken: cancellationToken);

        if (establishment == null)
        {
            throw new NotFoundException(nameof(Institution), request.EstablishmentId);
        }

        await RemoveCountry(establishment, cancellationToken);

        return Unit.Value;
    }

    private async Task RemoveCountry(Institution establishment, CancellationToken cancellationToken)
    {
        _context.Institutions.Remove(establishment);
        await _context.SaveChangesAsync(cancellationToken);
    }
}