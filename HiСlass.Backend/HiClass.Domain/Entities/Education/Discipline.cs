using HiClass.Domain.EntityConnections;

namespace HiClass.Domain.Entities.Education;

public class Discipline
{
    public Guid DisciplineId { get; set; }
    public string Title { get; set; } = string.Empty;
    public ICollection<ClassDiscipline> ClassDisciplines { get; set; } = new List<ClassDiscipline>();
    public ICollection<UserDiscipline> UserDisciplines { get; set; } = new List<UserDiscipline>();
}