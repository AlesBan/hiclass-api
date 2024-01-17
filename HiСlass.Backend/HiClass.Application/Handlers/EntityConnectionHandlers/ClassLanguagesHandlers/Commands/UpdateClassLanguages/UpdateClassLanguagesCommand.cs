using MediatR;

namespace HiClass.Application.Handlers.EntityConnectionHandlers.ClassLanguagesHandlers.Commands.
    UpdateClassLanguages;

public class UpdateClassLanguagesCommand : IRequest<Unit>
{
    public Guid ClassId { get; set; }
    public IEnumerable<Guid> NewLanguageIds { get; set; }
}