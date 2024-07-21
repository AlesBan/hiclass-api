using HiClass.Domain.Entities.Education;
using HiClass.Domain.Entities.Main;

namespace HiClass.Domain.EntityConnections;

public class UserGrade
{
    public Guid GradeId { get; set; }
    public Grade? Grade { get; set; } 
    public Guid UserId { get; set; }
    public User? User { get; set; } 
    
}