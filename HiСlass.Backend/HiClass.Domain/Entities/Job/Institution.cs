using HiClass.Domain.Entities.Main;
using HiClass.Domain.EntityConnections;

namespace HiClass.Domain.Entities.Job;

public class Institution
{
    public Guid InstitutionId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public ICollection<User> Users { get; set; } = new List<User>();
    public List<InstitutionTypeInstitution> InstitutionTypes { get; set; } = new();
}