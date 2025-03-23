using System.ComponentModel.DataAnnotations;
using HiClass.Application.Models.User.Authentication;
using HiClass.Domain.Entities.Main;
using MediatR;

namespace HiClass.Application.Handlers.EntityHandlers.UserHandlers.Commands.UpdateUserVerification;

public class UpdateUserVerificationAndRefreshTokenCommand : IRequest<TokenModelResponseDto>
{   
    [Required] public Guid UserId { get; set; } = Guid.Empty;
    public string? DeviceToken { get; set; } = string.Empty;
    [Required] public string Email { get; set; } = string.Empty;
    [Required] public string VerificationCode { get; set; } = null!;
    
    [Required] public string? RefreshToken { get; set; }
}