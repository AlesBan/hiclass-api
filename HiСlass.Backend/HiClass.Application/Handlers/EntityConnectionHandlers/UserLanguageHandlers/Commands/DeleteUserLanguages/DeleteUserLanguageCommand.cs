using HiClass.Domain.Entities.Education;
using HiClass.Domain.Entities.Main;
using MediatR;

namespace HiClass.Application.Handlers.EntityConnectionHandlers.UserLanguagesHandlers.Commands.DeleteUserLanguages;

public class DeleteUserLanguageCommand : IRequest<Unit>
{
    public Guid UserId { get; set; }
    public Guid LanguageId { get; set; }
}