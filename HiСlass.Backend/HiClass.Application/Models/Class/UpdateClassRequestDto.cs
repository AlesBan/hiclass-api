using Microsoft.AspNetCore.Http;

namespace HiClass.Application.Models.Class;

public class UpdateClassRequestDto
{
    public string Title { get; set; } = string.Empty;
    public int GradeNumber { get; set; }
    public IFormFile ImageFormFile  { get; set; } = null!;
    public IEnumerable<string> LanguageTitles { get; set; } = new List<string>();
    public IEnumerable<string> DisciplineTitles { get; set; } = new List<string>();
}