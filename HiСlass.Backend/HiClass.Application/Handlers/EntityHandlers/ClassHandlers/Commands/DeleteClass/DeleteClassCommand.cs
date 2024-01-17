using MediatR;

namespace HiClass.Application.Handlers.EntityHandlers.ClassHandlers.Commands.DeleteClass;

public class DeleteClassCommand : IRequest<Unit>
{
    public Guid ClassId { get; set; }

    public DeleteClassCommand(Guid classId)
    {
        ClassId = classId;
    }
}