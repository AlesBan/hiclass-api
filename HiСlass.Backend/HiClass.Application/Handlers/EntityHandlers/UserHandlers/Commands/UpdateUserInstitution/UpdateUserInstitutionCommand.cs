using System.ComponentModel.DataAnnotations;
using HiClass.Domain.Entities.Main;
using MediatR;

namespace HiClass.Application.Handlers.EntityHandlers.UserHandlers.Commands.UpdateUserInstitution;

public class UpdateUserInstitutionCommand : IRequest<User>
{
    [Required] public Guid UserId { get; set; }
    [Required] public string InstitutionTitle { get; set; }
    [Required] public string Address { get; set; }
    [Required] public IEnumerable<string> Types { get; set; }

}