using System.ComponentModel.DataAnnotations;

namespace HiClass.Application.Models.User.Editing.Requests;

public class EditPersonalInfoRequestDto
{
    [Required] public bool IsATeacher { get; set; }
    [Required] public bool IsAnExpert { get; set; }
    [Required] public string FirstName { get; set; } = string.Empty;
    [Required] public string LastName { get; set; } = string.Empty;
    [Required] public string CityTitle { get; set; } = string.Empty;
    [Required] public string CountryTitle { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}