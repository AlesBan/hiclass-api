using System.ComponentModel.DataAnnotations;
using HiClass.Domain.Entities.Main;
using MediatR;

namespace HiClass.Application.Handlers.EntityHandlers.UserHandlers.Commands.UpdateUserPasswordHash;

public class UpdateUserPasswordCommand : IRequest<User>
{
    [Required] public Guid UserId { get; set; }
    [Required] public string Password { get; set; }
}