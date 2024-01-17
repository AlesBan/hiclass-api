using HiClass.Application.Handlers.EntityConnectionHandlers.UserGradeHandlers.Commands.CreateUserGrade;
using HiClass.Application.Interfaces;
using MediatR;

namespace HiClass.Application.Handlers.EntityConnectionHandlers.UserGradeHandlers.Commands.UpdateUserGrades;

public class UpdateUserGradesCommandHandler : IRequestHandler<UpdateUserGradesCommand, Unit>
{
    private readonly ISharedLessonDbContext _context;
    private readonly IMediator _mediator;

    public UpdateUserGradesCommandHandler(ISharedLessonDbContext context, IMediator mediator)
    {
        _context = context;
        _mediator = mediator;
    }

    public async Task<Unit> Handle(UpdateUserGradesCommand request, CancellationToken cancellationToken)
    {
        _context.UserGrades
            .RemoveRange(_context.UserGrades
                .Where(ug =>
                    ug.User.UserId == request.UserId));

        await _context.SaveChangesAsync(cancellationToken);

        return await _mediator.Send(new CreateUserGradesCommand()
        {
            UserId = request.UserId,
            GradeIds = request.NewGradeIds
        }, cancellationToken);
    }
}