using System.ComponentModel.DataAnnotations;
using HiClass.Domain.Entities.Main;
using MediatR;

namespace HiClass.Application.Handlers.EntityHandlers.UserHandlers.Commands.SetUserPassword;

public class SetUserPasswordCommand: IRequest<User>
{
    [Required] public Guid UserId { get; set; }
    [Required] public string NewPassword { get; set; } = string.Empty;
}