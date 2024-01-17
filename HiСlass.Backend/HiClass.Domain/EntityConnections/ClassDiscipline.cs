using HiClass.Domain.Entities.Education;
using HiClass.Domain.Entities.Main;

namespace HiClass.Domain.EntityConnections;

public class ClassDiscipline
{
    public Guid DisciplineId { get; set; }
    public Discipline Discipline { get; set; }
    
    public Guid ClassId { get; set; }
    public Class Class { get; set; }
}