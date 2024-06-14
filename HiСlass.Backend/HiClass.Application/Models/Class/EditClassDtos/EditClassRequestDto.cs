using System.ComponentModel.DataAnnotations;

namespace HiClass.Application.Models.Class.EditClassDtos;

public class EditClassRequestDto
{
    [Required] public string Title { get; } = null!;
    [Required] public int GradeNumber { get; }
    [Required] public IEnumerable<string> LanguageTitles { get; } = null!;
    [Required] public IEnumerable<string> DisciplineTitles { get; } = null!;
}