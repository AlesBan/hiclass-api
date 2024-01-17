namespace HiClass.Application.Dtos.UserDtos.Update;

public class UpdateInstitutionRequestDto
{
    public string InstitutionTitle { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public IEnumerable<string> Types { get; set; } = new List<string>();
}