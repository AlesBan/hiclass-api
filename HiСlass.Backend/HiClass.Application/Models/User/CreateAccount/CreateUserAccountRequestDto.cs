using System.ComponentModel.DataAnnotations;
using HiClass.Application.Attributes;
using HiClass.Application.Models.Institution;
using Microsoft.AspNetCore.Http;

namespace HiClass.Application.Models.User.CreateAccount;

public class CreateUserAccountRequestDto
{
    [Required] public string FirstName { get; set; } = string.Empty;
    [Required] public string LastName { get; set; } = string.Empty;

    [Required]
    [AtLeastOneOfPositionTrue("IsATeacher", "IsAnExpert")]
    public bool IsATeacher { get; set; }

    [Required]
    [AtLeastOneOfPositionTrue("IsATeacher", "IsAnExpert")]
    public bool IsAnExpert { get; set; }
    [Required] public string CityLocation { get; set; } = string.Empty;
    [Required] public string CountryLocation { get; set; } = string.Empty;
    [Required] public InstitutionDto InstitutionDto { get; set; }
    [Required] public IEnumerable<string> DisciplineTitles { get; set; } = new List<string>();
    [Required] public IEnumerable<string> LanguageTitles { get; set; } = new List<string>();
    [Required] public IEnumerable<int> GradesEnumerable { get; set; } = new List<int>();
    [Required] public IFormFile ImageFormFile{ get; set; }
}