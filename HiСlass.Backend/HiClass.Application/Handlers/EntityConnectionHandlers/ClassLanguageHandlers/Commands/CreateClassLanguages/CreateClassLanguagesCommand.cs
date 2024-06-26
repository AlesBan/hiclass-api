using MediatR;

namespace HiClass.Application.Handlers.EntityConnectionHandlers.ClassLanguagesHandlers.Commands.
    CreateClassLanguages;

public class CreateClassLanguagesCommand : IRequest<Unit>
{
    public Guid ClassId { get; set; }
    public IEnumerable<Guid> LanguageIds { get; set; }
}