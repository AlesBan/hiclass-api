namespace HiClass.Application.Models.User.Editing.Requests;

public class EditProfessionalInfoRequestDto
{
    public IEnumerable<string> Languages { get; set; } = new List<string>();
    public IEnumerable<string> Disciplines { get; set; } = new List<string>();
    public IEnumerable<int> Grades { get; set; } = new List<int>();
}