using HiClass.Application.Models.Search;
using HiClass.Domain.Entities.Main;
using MediatR;

namespace HiClass.Application.Handlers.EntityHandlers.UserHandlers.Queries.GetUserProfileListByDefaultSearchRequest;

public class GetUserListByDefaultSearchRequestCommand : IRequest<IEnumerable<User>>
{
    public Guid UserId { get; init; }
    public IEnumerable<Guid> DisciplineIds { get; init; } = new List<Guid>();
    public Guid CountryId { get; init; }
}