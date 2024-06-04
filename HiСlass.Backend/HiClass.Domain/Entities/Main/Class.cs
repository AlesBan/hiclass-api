using HiClass.Domain.Entities.Communication;
using HiClass.Domain.Entities.Education;
using HiClass.Domain.EntityConnections;

namespace HiClass.Domain.Entities.Main;

public class Class
{
    public Guid ClassId { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; }
    public string Title { get; set; } = string.Empty;
    public Guid GradeId { get; set; }
    public Grade Grade { get; set; }
    public string? ImageUrl { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
    public ICollection<ClassLanguage> ClassLanguages { get; set; } = new List<ClassLanguage>();
    public ICollection<ClassDiscipline> ClassDisciplines { get; set; } = new List<ClassDiscipline>();
    public ICollection<Invitation> ReceivedInvitations { get; set; } = new List<Invitation>();
    public ICollection<Invitation> SentInvitations { get; set; } = new List<Invitation>();
    public ICollection<Feedback> SentFeedBacks { get; set; } = new List<Feedback>();
    public ICollection<Feedback> ReceivedFeedBacks { get; set; } = new List<Feedback>();
}