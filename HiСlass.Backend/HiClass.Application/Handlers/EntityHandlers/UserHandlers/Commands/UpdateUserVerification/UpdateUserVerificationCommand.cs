using System.ComponentModel.DataAnnotations;
using HiClass.Domain.Entities.Main;
using MediatR;

namespace HiClass.Application.Handlers.EntityHandlers.UserHandlers.Commands.UpdateUserVerification;

public class UpdateUserVerificationCommand : IRequest<string>
{
    [Required] public Guid UserId { get; set; } = Guid.Empty;
    [Required] public string Email { get; set; } = string.Empty;
    [Required] public string VerificationCode { get; set; } = null!;
}