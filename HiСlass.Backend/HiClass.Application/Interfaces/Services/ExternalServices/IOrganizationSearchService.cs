using HiClass.Application.Dtos.InstitutionDtos;

namespace HiClass.Application.Interfaces.Services.ExternalServices;

public interface IOrganizationSearchService
{
    public Task<IEnumerable<InstitutionSearchResponseDto>> GetInstitutions(InstitutionDto establishment);
}