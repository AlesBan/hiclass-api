using HiClass.Application.Common.Exceptions.Database;
using HiClass.Application.Interfaces;
using HiClass.Domain.Entities.Main;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HiClass.Application.Handlers.EntityHandlers.ClassHandlers.Commands.SetClassImage;

public class SetClassImageCommandHandler : IRequestHandler<SetClassImageCommand, string>
{
    private readonly ISharedLessonDbContext _context;

    public SetClassImageCommandHandler(ISharedLessonDbContext context)
    {
        _context = context;
    }

    public async Task<string> Handle(SetClassImageCommand request, CancellationToken cancellationToken)
    {
        var @class = await _context.Classes
            .FirstOrDefaultAsync(x =>
                x.ClassId == request.ClassId, cancellationToken: cancellationToken);

        if (@class == null)
        {
            throw new NotFoundException(nameof(Class), request.ClassId);
        }

        @class.ImageUrl = request.ImageUrl;

        _context.Classes.Attach(@class).State = EntityState.Modified;
        await _context.SaveChangesAsync(cancellationToken);
        
        return @class.ImageUrl;
    }
}