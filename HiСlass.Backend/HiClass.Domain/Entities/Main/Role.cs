using HiClass.Domain.EntityConnections;

namespace HiClass.Domain.Entities.Main;

public class Role
{
    public Guid RoleId { get; set; }
    public string Title { get; set; } = string.Empty;
    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}