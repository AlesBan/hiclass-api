using HiClass.Application.Handlers.EntityConnectionHandlers.UserDisciplinesHandlers.Commands.CreateUserDisciplines;
using HiClass.Application.Handlers.EntityConnectionHandlers.UserDisciplinesHandlers.Commands.UpdateUserDisciplines;
using HiClass.Application.Interfaces;
using MediatR;

namespace HiClass.Application.Handlers.EntityConnectionHandlers.UserDisciplineHandlers.Commands.
    UpdateUserDisciplines;

public class UpdateUserDisciplinesCommandHandler : IRequestHandler<UpdateUserDisciplinesCommand, Unit>
{
    private readonly ISharedLessonDbContext _context;
    private readonly IMediator _mediator;

    public UpdateUserDisciplinesCommandHandler(ISharedLessonDbContext context, IMediator mediator)
    {
        _context = context;
        _mediator = mediator;
    }

    public async Task<Unit> Handle(UpdateUserDisciplinesCommand request, CancellationToken cancellationToken)
    {
        _context.UserDisciplines
            .RemoveRange(_context.UserDisciplines
                .Where(ud =>
                    ud.UserId == request.UserId));
        
        await _context.SaveChangesAsync(cancellationToken);
        
        return await _mediator.Send(new CreateUserDisciplinesCommand
        {
            UserId = request.UserId,
            DisciplineIds = request.NewDisciplineIds
        }, cancellationToken);
    }
}