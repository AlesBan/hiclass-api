using HiClass.Domain.Entities.Education;
using HiClass.Domain.Entities.Main;

namespace HiClass.Domain.EntityConnections;

public class UserDiscipline
{
    public Guid DisciplineId { get; set; }
    public Discipline? Discipline { get; set; }
    public Guid UserId { get; set; }
    public User? User { get; set; }
}