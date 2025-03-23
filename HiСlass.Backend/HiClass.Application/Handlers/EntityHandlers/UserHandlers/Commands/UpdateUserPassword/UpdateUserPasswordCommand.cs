using System.ComponentModel.DataAnnotations;
using HiClass.Application.Models.User.Authentication;
using MediatR;

namespace HiClass.Application.Handlers.EntityHandlers.UserHandlers.Commands.UpdateUserPassword;

public class UpdateUserPasswordCommand : IRequest<TokenModelResponseDto>
{
    public string DeviceToken { get; set; } = string.Empty;
    [Required] public Guid UserId { get; set; }
    [Required] public string NewPassword { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
}