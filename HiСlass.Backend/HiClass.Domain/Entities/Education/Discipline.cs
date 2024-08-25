using HiClass.Domain.Entities.Main;
using HiClass.Domain.EntityConnections;

namespace HiClass.Domain.Entities.Education;

public class Discipline
{
    public Guid DisciplineId { get; set; }
    public string Title { get; set; } = string.Empty;
    public ICollection<Class> Classes { get; set; } = new List<Class>();
    public ICollection<UserDiscipline> UserDisciplines { get; set; } = new List<UserDiscipline>();
}