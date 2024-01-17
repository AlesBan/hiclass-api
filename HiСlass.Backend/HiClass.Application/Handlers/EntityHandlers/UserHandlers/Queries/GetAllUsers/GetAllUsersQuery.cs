using HiClass.Domain.Entities.Main;
using MediatR;

namespace HiClass.Application.Handlers.EntityHandlers.UserHandlers.Queries.GetAllUsers;

public class GetAllUsersQuery : IRequest<List<User>>
{
}