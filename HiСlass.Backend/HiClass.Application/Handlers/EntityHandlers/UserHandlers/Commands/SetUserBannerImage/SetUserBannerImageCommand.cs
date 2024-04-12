using System.ComponentModel.DataAnnotations;
using MediatR;

namespace HiClass.Application.Handlers.EntityHandlers.UserHandlers.Commands.SetUserBannerImage;

public class SetUserBannerImageCommand : IRequest<string>
{
    [Required] public Guid UserId { get; set; }

    [Required] public string ImageUrl { get; set; } = null!;
}