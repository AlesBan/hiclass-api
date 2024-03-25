using System.ComponentModel.DataAnnotations;
using HiClass.Domain.Entities.Main;
using MediatR;

namespace HiClass.Application.Handlers.EntityHandlers.UserHandlers.Commands.EditProfessionalInfo;

public class EditProfessionalInfoCommand : IRequest<User>
{
    [Required] public Guid UserId { get; set; }
    [Required] public IEnumerable<string> LanguageTitles { get; set; }
    [Required] public IEnumerable<string> DisciplineTitles { get; set; }
    [Required] public IEnumerable<int> GradeNumbers { get; set; }
}