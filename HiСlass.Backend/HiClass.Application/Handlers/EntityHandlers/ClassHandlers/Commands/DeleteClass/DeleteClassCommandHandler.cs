using HiClass.Application.Common.Exceptions.Database;
using HiClass.Application.Interfaces;
using HiClass.Domain.Entities.Main;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HiClass.Application.Handlers.EntityHandlers.ClassHandlers.Commands.DeleteClass;

public class DeleteClassCommandHandler : IRequestHandler<DeleteClassCommand, Unit>
{
    private readonly ISharedLessonDbContext _context;

    public DeleteClassCommandHandler(ISharedLessonDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteClassCommand request, CancellationToken cancellationToken)
    {
        var @class = await _context.Classes
            .SingleOrDefaultAsync(x => x.ClassId == request.ClassId, cancellationToken: cancellationToken);
        
        if (@class == null)
        {
            throw new NotFoundException(nameof(Class), request.ClassId);
        }
        
        _context.Classes.Remove(@class);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}