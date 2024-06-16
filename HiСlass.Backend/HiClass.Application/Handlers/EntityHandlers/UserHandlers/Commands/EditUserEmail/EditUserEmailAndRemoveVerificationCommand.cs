using System.ComponentModel.DataAnnotations;
using HiClass.Domain.Entities.Main;
using MediatR;

namespace HiClass.Application.Handlers.EntityHandlers.UserHandlers.Commands.EditUserEmail;

public class EditUserEmailAndRemoveVerificationCommand : IRequest<User>
{
    [Required] public Guid UserId { get; set; }
    [Required] public string Email { get; set; } = string.Empty;
}