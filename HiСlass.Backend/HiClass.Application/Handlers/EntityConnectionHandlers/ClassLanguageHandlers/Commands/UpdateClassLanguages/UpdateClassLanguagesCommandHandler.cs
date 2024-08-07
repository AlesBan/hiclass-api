using HiClass.Application.Handlers.EntityConnectionHandlers.ClassLanguageHandlers.Commands.CreateClassLanguages;
using HiClass.Application.Handlers.EntityConnectionHandlers.ClassLanguageHandlers.Commands.UpdateClassLanguages;
using HiClass.Application.Handlers.EntityConnectionHandlers.ClassLanguagesHandlers.Commands.CreateClassLanguages;
using HiClass.Application.Interfaces;
using MediatR;

namespace HiClass.Application.Handlers.EntityConnectionHandlers.ClassLanguagesHandlers.Commands.
    UpdateClassLanguages;

public class UpdateClassLanguagesCommandHandler : IRequestHandler<UpdateClassLanguagesCommand, Unit>
{
    private readonly ISharedLessonDbContext _context;
    private readonly IMediator _mediator;

    public UpdateClassLanguagesCommandHandler(ISharedLessonDbContext context, IMediator mediator)
    {
        _context = context;
        _mediator = mediator;
    }

    public async Task<Unit> Handle(UpdateClassLanguagesCommand request, CancellationToken cancellationToken)
    {
        var classLanguages = _context.ClassLanguages
            .Where(cl =>
                cl.ClassId == request.ClassId);

        _context.ClassLanguages
            .RemoveRange(classLanguages);
        
        await _context.SaveChangesAsync(cancellationToken);

        return await _mediator.Send(new CreateClassLanguagesCommand()
        {
            ClassId = request.ClassId,
            LanguageIds = request.NewLanguageIds
        }, cancellationToken);
    }
}