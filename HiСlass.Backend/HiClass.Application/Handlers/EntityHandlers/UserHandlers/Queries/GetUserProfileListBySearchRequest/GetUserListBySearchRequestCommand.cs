using HiClass.Application.Dtos.SearchDtos;
using HiClass.Domain.Entities.Main;
using MediatR;

namespace HiClass.Application.Handlers.EntityHandlers.UserHandlers.Queries.GetUserProfileListBySearchRequest;

public class GetUserListBySearchRequestCommand : IRequest<IEnumerable<User>>
{
    public SearchCommandDto SearchRequest { get; set; }
}