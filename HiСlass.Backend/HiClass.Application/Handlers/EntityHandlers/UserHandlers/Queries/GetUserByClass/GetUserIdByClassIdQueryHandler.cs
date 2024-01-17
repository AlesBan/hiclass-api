using HiClass.Application.Handlers.EntityHandlers.ClassHandlers.Queries.GetClass;
using HiClass.Application.Interfaces;
using MediatR;

namespace HiClass.Application.Handlers.EntityHandlers.UserHandlers.Queries.GetUserByClass;

public class GetUserIdByClassIdQueryHandler : IRequestHandler<GetUserIdByClassIdQuery, Guid>
{
    private readonly ISharedLessonDbContext _context;
    private readonly IMediator _mediator;

    public GetUserIdByClassIdQueryHandler(ISharedLessonDbContext context, IMediator mediator)
    {
        _context = context;
        _mediator = mediator;
    }

    public async Task<Guid> Handle(GetUserIdByClassIdQuery request, CancellationToken cancellationToken)
    {
        var userClass = await _mediator.Send(new GetClassByIdQuery(request.ClassId), cancellationToken);
        return userClass.UserId;
    }
}