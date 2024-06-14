using System.ComponentModel.DataAnnotations;

namespace HiClass.Application.Models.Class.EditClassDtos;

public class EditClassRequestDto
{
    [Required] public string Title { get; set; } = string.Empty;
    [Required] public int GradeNumber { get; set; }
    [Required] public IEnumerable<string> LanguageTitles { get; set; } = new List<string>();
    [Required] public IEnumerable<string> DisciplineTitles { get; set; } = new List<string>();
}