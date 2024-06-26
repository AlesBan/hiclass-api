using MediatR;

namespace HiClass.Application.Handlers.EntityConnectionHandlers.UserLanguagesHandlers.Commands.UpdateUserLanguages;

public class UpdateUserLanguagesCommand : IRequest<Unit>
{
    public Guid UserId { get; set; }
    public IEnumerable<Guid> NewLanguageIds { get; set; } = new List<Guid>();
}