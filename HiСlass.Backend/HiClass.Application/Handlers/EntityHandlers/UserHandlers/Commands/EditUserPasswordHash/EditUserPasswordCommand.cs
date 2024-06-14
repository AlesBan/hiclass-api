using System.ComponentModel.DataAnnotations;
using HiClass.Domain.Entities.Main;
using MediatR;

namespace HiClass.Application.Handlers.EntityHandlers.UserHandlers.Commands.EditUserPasswordHash;

public class EditUserPasswordCommand : IRequest<User>
{
    [Required] public Guid UserId { get; set; }
    [Required] public string OldPassword { get; set; } = string.Empty;
    [Required] public string NewPassword { get; set; } = string.Empty;
}