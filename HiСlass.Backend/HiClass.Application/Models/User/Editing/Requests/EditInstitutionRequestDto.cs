namespace HiClass.Application.Models.User.Editing.Requests;

public class EditInstitutionRequestDto
{
    public string InstitutionTitle { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public IEnumerable<string> Types { get; set; } = new List<string>();
}