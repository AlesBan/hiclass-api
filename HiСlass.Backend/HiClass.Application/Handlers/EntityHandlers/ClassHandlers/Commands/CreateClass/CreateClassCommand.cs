using HiClass.Domain.Entities.Main;
using MediatR;

namespace HiClass.Application.Handlers.EntityHandlers.ClassHandlers.Commands.CreateClass;

public class CreateClassCommand : IRequest<Class>
{
    public Guid ClassId { get; set; }
    public Guid UserId { get; set; }
    public string Title { get; set; } = string.Empty;
    public int GradeNumber { get; set; }
    public IEnumerable<Guid> DisciplineIds { get; set; } = new List<Guid>();
    public IEnumerable<Guid> LanguageIds { get; set; } = new List<Guid>();
    public string? ImageUrl { get; set; }
}