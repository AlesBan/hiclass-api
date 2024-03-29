using MediatR;

namespace HiClass.Application.Handlers.EntityHandlers.UserHandlers.Queries.GetUserByClass;

public class GetUserIdByClassIdQuery : IRequest<Guid>
{
    public Guid ClassId { get; set; }

    public GetUserIdByClassIdQuery(Guid classId)
    {
        ClassId = classId;
    }
}