using System.ComponentModel.DataAnnotations;
using HiClass.Domain.Entities.Main;
using MediatR;

namespace HiClass.Application.Handlers.EntityHandlers.UserHandlers.Commands.UpdateUserVerificationCode;

public class UpdateUserVerificationCodeCommand : IRequest<string>
{
    [Required] public string Email { get; set; } = string.Empty;
}