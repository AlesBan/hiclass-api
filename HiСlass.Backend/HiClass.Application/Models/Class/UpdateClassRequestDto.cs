using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace HiClass.Application.Models.Class;

public class UpdateClassRequestDto
{
    public UpdateClassRequestDto(int gradeNumber)
    {
        GradeNumber = gradeNumber;
    }

    [Required] public string Title { get; } = null!;
    [Required] public int GradeNumber { get; }
    [Required] public IFormFile FormFileImage { get; } = null!;
    [Required] public IEnumerable<string> LanguageTitles { get; } = null!;
    [Required] public IEnumerable<string> DisciplineTitles { get; } = null!;
}