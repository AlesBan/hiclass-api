using System.ComponentModel.DataAnnotations;

namespace HiClass.Application.Models.Class.EditClassDtos;

public class EditClassRequestDto
{
    [Required] public string Title { get; set;} = null!;
    [Required] public int GradeNumber { get; set;}
    [Required] public IEnumerable<string> LanguageTitles { get; set;} = null!;
    [Required] public IEnumerable<string> DisciplineTitles { get; set;} = null!;
}