using System.ComponentModel.DataAnnotations;
using HiClass.Domain.Entities.Main;
using MediatR;

namespace HiClass.Application.Handlers.EntityHandlers.UserHandlers.Commands.EditUserAccessToken;

public class EditUserAccessTokenCommand : IRequest<User>
{
    [Required] public Guid UserId { get; set; }
    [Required] public string AccessToken { get; set; } = null!;
}