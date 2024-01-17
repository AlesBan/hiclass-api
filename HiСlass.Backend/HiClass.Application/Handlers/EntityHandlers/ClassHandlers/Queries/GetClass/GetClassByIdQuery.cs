using HiClass.Domain.Entities.Main;
using MediatR;

namespace HiClass.Application.Handlers.EntityHandlers.ClassHandlers.Queries.GetClass;

public class GetClassByIdQuery : IRequest<Class>
{
    public Guid ClassId { get; set; }

    public GetClassByIdQuery(Guid classId)
    {
        ClassId = classId;
    }
}