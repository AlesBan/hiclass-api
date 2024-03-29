using System.ComponentModel.DataAnnotations;
using HiClass.Domain.Entities.Main;
using MediatR;

namespace HiClass.Application.Handlers.EntityHandlers.UserHandlers.Commands.UpdateUserVerificationCode;

public class UpdateUserVerificationCodeCommand : IRequest<User>
{
    [Required] public Guid UserId { get; set; }

    [Required] public string VerificationCode { get; set; } = null!;
}