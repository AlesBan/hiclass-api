using MediatR;

namespace HiClass.Application.Handlers.EntityConnectionHandlers.ClassLanguageHandlers.Commands.
    CreateClassLanguages;

public class CreateClassLanguagesCommand : IRequest<Unit>
{
    public Guid ClassId { get; set; }
    public IEnumerable<Guid> LanguageIds { get; set; } = null!;
}