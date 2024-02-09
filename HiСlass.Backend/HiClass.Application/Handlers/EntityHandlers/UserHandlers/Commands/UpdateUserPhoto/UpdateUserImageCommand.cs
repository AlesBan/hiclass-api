using MediatR;

namespace HiClass.Application.Handlers.EntityHandlers.UserHandlers.Commands.UpdateUserPhoto;

public class UpdateUserImageCommand : IRequest<Unit>
{
    public Guid UserId { get; set; }
    public string NewPhotoUrl { get; set; }

    public UpdateUserImageCommand(Guid userId, string newPhotoUrl)
    {
        UserId = userId;
        NewPhotoUrl = newPhotoUrl;
    }
}