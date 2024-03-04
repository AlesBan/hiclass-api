using System.ComponentModel.DataAnnotations;

namespace HiClass.Application.Models.Class.UpdateClassDtos;

public class UpdateClassRequestDto
{
    public UpdateClassRequestDto(int gradeNumber)
    {
        GradeNumber = gradeNumber;
    }

    [Required] public string Title { get; } = null!;
    [Required] public int GradeNumber { get; }
    [Required] public IEnumerable<string> LanguageTitles { get; } = null!;
    [Required] public IEnumerable<string> DisciplineTitles { get; } = null!;
}