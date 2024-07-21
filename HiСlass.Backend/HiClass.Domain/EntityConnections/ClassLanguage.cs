using HiClass.Domain.Entities.Education;
using HiClass.Domain.Entities.Main;

namespace HiClass.Domain.EntityConnections;

public class ClassLanguage
{
    public Guid LanguageId { get; set; }
    public Language? Language { get; set; }
    public Guid ClassId { get; set; }
    public Class? Class { get; set; }
}