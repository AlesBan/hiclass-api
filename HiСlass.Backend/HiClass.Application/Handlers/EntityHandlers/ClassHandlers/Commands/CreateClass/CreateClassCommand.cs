using System.ComponentModel.DataAnnotations;
using HiClass.Domain.Entities.Main;
using MediatR;

namespace HiClass.Application.Handlers.EntityHandlers.ClassHandlers.Commands.CreateClass;

public class CreateClassCommand : IRequest<Class>
{
    [Required] public Guid ClassId { get; set; }
    [Required] public Guid UserId { get; set; }
    [Required] public string Title { get; set; } = string.Empty;
    [Required] public int GradeNumber { get; set; }
    [Required] public Guid DisciplineId { get; set; }
    [Required] public IEnumerable<Guid> LanguageIds { get; set; } = new List<Guid>();
}