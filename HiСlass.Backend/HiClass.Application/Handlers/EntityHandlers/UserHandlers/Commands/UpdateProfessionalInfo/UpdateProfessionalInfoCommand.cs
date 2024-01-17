using HiClass.Domain.Entities.Main;
using MediatR;

namespace HiClass.Application.Handlers.EntityHandlers.UserHandlers.Commands.UpdateProfessionalInfo;

public class UpdateProfessionalInfoCommand : IRequest<User>
{
    public Guid UserId { get; set; }
    public IEnumerable<string> LanguageTitles { get; set; } 
    public IEnumerable<string> DisciplineTitles { get; set; } 
    public IEnumerable<int> GradeNumbers{ get; set; }


    public UpdateProfessionalInfoCommand(Guid userId, IEnumerable<string> languageTitles, IEnumerable<string> disciplineTitles, IEnumerable<int> gradeNumbers)
    {
        UserId = userId;
        LanguageTitles = languageTitles;
        DisciplineTitles = disciplineTitles;
        GradeNumbers = gradeNumbers;
    }
}