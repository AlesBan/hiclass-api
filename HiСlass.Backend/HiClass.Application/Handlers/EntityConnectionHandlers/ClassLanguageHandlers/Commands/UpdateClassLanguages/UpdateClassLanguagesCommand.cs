using System.ComponentModel.DataAnnotations;
using MediatR;

namespace HiClass.Application.Handlers.EntityConnectionHandlers.ClassLanguageHandlers.Commands.
    UpdateClassLanguages;

public class UpdateClassLanguagesCommand : IRequest<Unit>
{
    [Required] public Guid ClassId { get; set; }
    [Required] public IEnumerable<Guid> NewLanguageIds { get; set; } = null!;
}