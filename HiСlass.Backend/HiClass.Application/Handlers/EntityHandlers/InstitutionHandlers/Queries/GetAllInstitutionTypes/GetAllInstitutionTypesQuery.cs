using MediatR;

namespace HiClass.Application.Handlers.EntityHandlers.InstitutionHandlers.Queries.GetAllInstitutionTypes;

public class GetAllInstitutionTypesQuery : IRequest<List<string>>
{
}