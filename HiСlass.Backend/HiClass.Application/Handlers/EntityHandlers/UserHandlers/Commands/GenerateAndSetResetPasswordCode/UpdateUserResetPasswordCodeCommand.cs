using System.ComponentModel.DataAnnotations;
using MediatR;

namespace HiClass.Application.Handlers.EntityHandlers.UserHandlers.Commands.GenerateAndSetResetPasswordCode;

public class UpdateUserResetPasswordCodeCommand : IRequest<string>
{
    [Required] public string Email { get; set; } = string.Empty;
}