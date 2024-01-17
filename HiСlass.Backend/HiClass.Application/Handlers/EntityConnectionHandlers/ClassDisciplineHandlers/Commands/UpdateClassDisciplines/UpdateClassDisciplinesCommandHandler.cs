using HiClass.Application.Handlers.EntityConnectionHandlers.ClassDisciplineHandlers.Commands.CreateClassDisciplines;
using HiClass.Application.Interfaces;
using MediatR;

namespace HiClass.Application.Handlers.EntityConnectionHandlers.ClassDisciplineHandlers.Commands.UpdateClassDisciplines;

public class UpdateClassDisciplinesCommandHandler: IRequestHandler<UpdateClassDisciplinesCommand, Unit>
{
    private readonly ISharedLessonDbContext _context;
    private readonly IMediator _mediator;

    public UpdateClassDisciplinesCommandHandler(ISharedLessonDbContext context, IMediator mediator)
    {
        _context = context;
        _mediator = mediator;
    }

    public async Task<Unit> Handle(UpdateClassDisciplinesCommand request, CancellationToken cancellationToken)
    {
        var classDisciplines = _context.ClassDisciplines
            .Where(cl => 
                cl.ClassId == request.ClassId);

        _context.ClassDisciplines
            .RemoveRange(classDisciplines);
        
        await _context.SaveChangesAsync(cancellationToken);
        
        return await _mediator.Send(new CreateClassDisciplinesCommand()
        {
            ClassId = request.ClassId,
            DisciplineIds = request.NewDisciplineIds
            
        }, cancellationToken);
    }
}