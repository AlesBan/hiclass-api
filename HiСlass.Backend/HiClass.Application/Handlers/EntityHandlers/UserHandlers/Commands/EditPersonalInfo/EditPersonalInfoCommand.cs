using System.ComponentModel.DataAnnotations;
using HiClass.Domain.Entities.Main;
using MediatR;

namespace HiClass.Application.Handlers.EntityHandlers.UserHandlers.Commands.EditPersonalInfo;

public class EditPersonalInfoCommand : IRequest<User>
{
    [Required] public Guid UserId { get; set; }
    [Required] public bool IsATeacher { get; set; }
    [Required] public bool IsAnExpert { get; set; }
    [Required] public string FirstName { get; set; } = string.Empty;
    [Required] public string LastName { get; set; } = string.Empty;
    [Required] public string CityTitle { get; set; } = string.Empty;
    [Required] public string CountryTitle { get; set; } = string.Empty;
    [Required] public string Description { get; set; } = string.Empty;
}