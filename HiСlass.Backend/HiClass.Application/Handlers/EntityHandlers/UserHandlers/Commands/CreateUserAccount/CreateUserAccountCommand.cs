using HiClass.Domain.Entities.Main;
using MediatR;

namespace HiClass.Application.Handlers.EntityHandlers.UserHandlers.Commands.CreateUserAccount;

public class CreateUserAccountCommand : IRequest<User>
{
    public Guid UserId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public bool IsATeacher { get; set; }
    public bool IsAnExpert { get; set; }
    public Guid CityId { get; set; }
    public Guid CountryId { get; set; }
    public Guid InstitutionId { get; set; }
    public IEnumerable<Guid> DisciplineIds { get; set; } = new List<Guid>();
    public IEnumerable<Guid> LanguageIds { get; set; } = new List<Guid>();
    public IEnumerable<Guid> GradeIds { get; set; } = new List<Guid>();
    public string ImageUrl { get; set; } = string.Empty;
}