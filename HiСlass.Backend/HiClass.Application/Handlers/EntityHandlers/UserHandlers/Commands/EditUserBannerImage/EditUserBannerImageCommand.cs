using HiClass.Domain.Entities.Main;
using MediatR;

namespace HiClass.Application.Handlers.EntityHandlers.UserHandlers.Commands.EditUserBannerImage;

public class EditUserBannerImageCommand : IRequest<User>
{
    public Guid UserId { get; set; }
    public string ImageUrl { get; set; }

    public EditUserBannerImageCommand(Guid userId, string imageUrl)
    {
        UserId = userId;
        ImageUrl = imageUrl;
    }
}