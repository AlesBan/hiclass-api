using System.ComponentModel.DataAnnotations;
using MediatR;

namespace HiClass.Application.Handlers.EntityHandlers.UserHandlers.Commands.SetUserImage;

public class SetUserImageCommand : IRequest<string>
{
    [Required] public Guid UserId { get; set; }

    [Required] public string ImageUrl { get; set; } = null!;
}