using Microsoft.AspNetCore.Http;

namespace HiClass.Application.Models.Class
{
    public class CreateClassRequestDto
    {
        public string Title { get; set; } = string.Empty;
        public int GradeNumber { get; set; }
        public IFormFile FormFileImage { get; set; }
        public IEnumerable<string> LanguageTitles { get; set; } = new List<string>();
        public IEnumerable<string> DisciplineTitles { get; set; } = new List<string>();
    }
}