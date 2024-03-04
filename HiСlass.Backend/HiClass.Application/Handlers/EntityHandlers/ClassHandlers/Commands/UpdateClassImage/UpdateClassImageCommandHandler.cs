using HiClass.Application.Common.Exceptions.Database;
using HiClass.Application.Interfaces;
using HiClass.Domain.Entities.Main;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HiClass.Application.Handlers.EntityHandlers.ClassHandlers.Commands.UpdateClassImage;

public class UpdateClassImageCommandHandler : IRequestHandler<UpdateClassImageCommand, string>
{
    private readonly ISharedLessonDbContext _context;

    public UpdateClassImageCommandHandler(ISharedLessonDbContext context)
    {
        _context = context;
    }

    public async Task<string> Handle(UpdateClassImageCommand request, CancellationToken cancellationToken)
    {
        var @class = await _context.Classes
            .FirstOrDefaultAsync(c => c.ClassId == request.ClassId, cancellationToken: cancellationToken);
        
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