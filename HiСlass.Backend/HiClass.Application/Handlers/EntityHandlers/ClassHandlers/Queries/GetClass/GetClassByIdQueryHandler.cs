using HiClass.Application.Common.Exceptions.Database;
using HiClass.Application.Interfaces;
using HiClass.Domain.Entities.Main;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HiClass.Application.Handlers.EntityHandlers.ClassHandlers.Queries.GetClass;

public class GetClassByIdQueryHandler : IRequestHandler<GetClassByIdQuery, Class>
{
    private readonly ISharedLessonDbContext _context;

    public GetClassByIdQueryHandler(ISharedLessonDbContext context)
    {
        _context = context;
    }

    public async Task<Class> Handle(GetClassByIdQuery request, CancellationToken cancellationToken)
    {
        var @class = await _context.Classes
            .Include(c => c.User)
            .ThenInclude(u => u.ReceivedFeedbacks)
            .Include(c => c.Grade)
            .Include(c => c.ClassDisciplines)
            .ThenInclude(cd => cd.Discipline)
            .Include(c => c.ClassLanguages)
            .ThenInclude(cl => cl.Language)
            .SingleOrDefaultAsync(c =>
                c.ClassId == request.ClassId, cancellationToken: cancellationToken);

        if (@class == null)
        {
            throw new NotFoundException(nameof(Class), request.ClassId);
        }

        return @class;
    }
}