using HiClass.Domain.Entities.Main;
using MediatR;

namespace HiClass.Application.Handlers.EntityHandlers.UserHandlers.Commands.UpdateUserImage;

public class UpdateUserImageCommand : IRequest<User>
{
    public Guid UserId { get; set; }
    public string ImageUrl { get; set; }

    public UpdateUserImageCommand(Guid userId, string imageUrl)
    {
        UserId = userId;
        ImageUrl = imageUrl;
    }
}