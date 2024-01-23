using HiClass.Domain.Entities.Main;
using MediatR;

namespace HiClass.Application.Handlers.EntityHandlers.UserHandlers.Commands.UpdateUserPhoto;

public class UpdateUserPhotoCommand : IRequest<Unit>
{
    public Guid UserId { get; set; }
    public string NewPhotoUrl { get; set; }
}