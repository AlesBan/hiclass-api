using HiClass.Domain.Entities.Education;
using HiClass.Domain.Entities.Main;

namespace HiClass.Domain.EntityConnections;

public class UserLanguage
{
    public Guid UserId { get; set; }
    public User User { get; set; }

    public Guid LanguageId { get; set; }
    public Language Language { get; set; }
}