using HiClass.Domain.EntityConnections;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HiClass.Persistence.EntityConnectionsConfiguration;

public class UserDisciplineConfiguration : IEntityTypeConfiguration<UserDiscipline>
{
    public void Configure(EntityTypeBuilder<UserDiscipline> builder)
    {
        builder.HasKey(td => new { td.UserId, td.DisciplineId });
        builder.HasIndex(td => new { td.UserId, td.DisciplineId })
            .IsUnique();

        builder.HasOne(td => td.User)
            .WithMany(t => t.UserDisciplines)
            .HasForeignKey(td => td.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasOne(td => td.Discipline)
            .WithMany(d => d.UserDisciplines)
            .HasForeignKey(td => td.DisciplineId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}