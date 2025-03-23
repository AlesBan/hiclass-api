using System.ComponentModel.DataAnnotations;
using HiClass.Application.Models.User.Authentication;
using MediatR;

namespace HiClass.Application.Handlers.EntityHandlers.UserHandlers.Commands.CreateUserAccount;

public class CreateUserAccountCommand : IRequest<TokenModelResponseDto>
{
    [Required] public Guid UserId { get; set; }
    [Required] public string FirstName { get; set; } = null!;
    [Required] public string LastName { get; set; } = null!;
    [Required] public bool IsATeacher { get; set; }
    [Required] public bool IsAnExpert { get; set; }
    [Required] public Guid CityId { get; set; }
    [Required] public Guid CountryId { get; set; }
    [Required] public Guid InstitutionId { get; set; }
    [Required] public IEnumerable<Guid> DisciplineIds { get; set; } = new List<Guid>();
    [Required] public IEnumerable<Guid> LanguageIds { get; set; } = new List<Guid>();
    [Required] public IEnumerable<Guid> GradeIds { get; set; } = new List<Guid>();
    public string DeviceToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
}