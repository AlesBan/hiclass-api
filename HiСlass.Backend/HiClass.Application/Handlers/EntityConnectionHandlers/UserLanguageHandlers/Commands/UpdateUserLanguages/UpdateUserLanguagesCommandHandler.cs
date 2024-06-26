using HiClass.Application.Handlers.EntityConnectionHandlers.UserLanguagesHandlers.Commands.CreateUserLanguages;
using HiClass.Application.Interfaces;
using MediatR;

namespace HiClass.Application.Handlers.EntityConnectionHandlers.UserLanguagesHandlers.Commands.UpdateUserLanguages;

public class UpdateUserLanguagesCommandHandler : IRequestHandler<UpdateUserLanguagesCommand, Unit>
{
    private readonly ISharedLessonDbContext _context;
    private readonly IMediator _mediator;

    public UpdateUserLanguagesCommandHandler(ISharedLessonDbContext context, IMediator mediator)
    {
        _context = context;
        _mediator = mediator;
    }

    public async Task<Unit> Handle(UpdateUserLanguagesCommand request, CancellationToken cancellationToken)
    {
        var userLanguages = _context.UserLanguages
            .Where(ul => 
                ul.UserId == request.UserId);

        _context.UserLanguages
            .RemoveRange(userLanguages);
        await _context.SaveChangesAsync(cancellationToken);
        
        return await _mediator.Send(new CreateUserLanguagesCommand()
        {
            UserId = request.UserId,
            LanguageIds = request.NewLanguageIds
            
        }, cancellationToken);
    }
}