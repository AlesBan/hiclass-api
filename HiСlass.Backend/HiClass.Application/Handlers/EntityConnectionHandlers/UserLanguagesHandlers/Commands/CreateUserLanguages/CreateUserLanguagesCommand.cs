using MediatR;

namespace HiClass.Application.Handlers.EntityConnectionHandlers.UserLanguagesHandlers.Commands.CreateUserLanguages;

public class CreateUserLanguagesCommand : IRequest<Unit>
{
    public Guid UserId { get; set; }
    public IEnumerable<Guid> LanguageIds { get; set; }

}