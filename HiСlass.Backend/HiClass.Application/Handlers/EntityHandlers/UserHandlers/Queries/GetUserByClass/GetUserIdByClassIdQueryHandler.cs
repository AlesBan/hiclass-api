using HiClass.Application.Common.Exceptions.Database;
using HiClass.Application.Handlers.EntityHandlers.ClassHandlers.Queries.GetClass;
using HiClass.Application.Interfaces;
using HiClass.Domain.Entities.Main;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HiClass.Application.Handlers.EntityHandlers.UserHandlers.Queries.GetUserByClass;

public class GetUserIdByClassIdQueryHandler : IRequestHandler<GetUserIdByClassIdQuery, Guid>
{
    private readonly ISharedLessonDbContext _context;

    public GetUserIdByClassIdQueryHandler(ISharedLessonDbContext context, IMediator mediator)
    {
        _context = context;
    }

    public async Task<Guid> Handle(GetUserIdByClassIdQuery request, CancellationToken cancellationToken)
    {
        var userClass = await _context.Classes
            .FirstOrDefaultAsync(x => x.ClassId == request.ClassId, cancellationToken);

        if (userClass == null)
        {
            throw new NotFoundException(nameof(Class), request.ClassId);
        }
        return userClass.UserId;
    }
}