using HiClass.Domain.Entities.Job;

namespace HiClass.Domain.EntityConnections;

public class InstitutionTypeInstitution
{
    public Guid InstitutionTypeId { get; set; }
    public InstitutionType InstitutionType { get; set; }
    
    public Guid InstitutionId { get; set; }
    public Institution Institution { get; set; }
}
