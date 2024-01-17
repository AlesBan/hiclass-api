using System.ComponentModel.DataAnnotations;
using HiClass.Domain.Entities.Main;
using MediatR;

namespace HiClass.Application.Handlers.EntityHandlers.UserHandlers.Commands.UpdateUserEmail;

public class UpdateUserEmailAndRemoveVerificationCommand : IRequest<User>
{
    [Required] public Guid UserId { get; set; }
    [Required] public string Email { get; set; }
}