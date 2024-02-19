using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace HiClass.Application.Models.Class
{
    public class CreateClassRequestDto
    {
        [Required] public string Title { get; init; } = null!;
        [Required] public int GradeNumber { get; init; }
        [Required] public IFormFile FormFileImage { get; init; } = null!;
        [Required] public IEnumerable<string> LanguageTitles { get; init; } = null!;
        [Required] public IEnumerable<string> DisciplineTitles { get; init; } = null!;
    }
}