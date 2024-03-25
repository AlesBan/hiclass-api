using HiClass.Domain.Entities.Main;
using MediatR;

namespace HiClass.Application.Handlers.EntityHandlers.UserHandlers.Commands.EditUserImage;

public class EditUserImageCommand : IRequest<User>
{
    public Guid UserId { get; set; }
    public string ImageUrl { get; set; }

    public EditUserImageCommand(Guid userId, string imageUrl)
    {
        UserId = userId;
        ImageUrl = imageUrl;
    }
}