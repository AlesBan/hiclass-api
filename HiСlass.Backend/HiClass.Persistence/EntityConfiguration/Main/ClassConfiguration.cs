using HiClass.Domain.Entities.Main;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HiClass.Persistence.EntityConfiguration.Main;

public class ClassConfiguration : IEntityTypeConfiguration<Class>
{
    public void Configure(EntityTypeBuilder<Class> builder)
    {
        builder.HasKey(c => c.ClassId);
        builder.HasIndex(c => c.ClassId)
            .IsUnique();

        builder.HasOne(c => c.Grade)
            .WithMany(g => g.Classes)
            .HasForeignKey(c => c.GradeId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(c => c.Title)
            .HasMaxLength(40)
            .IsRequired();

        builder.Property(c => c.ImageUrl)
            .HasMaxLength(200)
            .HasDefaultValue(string.Empty);

        builder.Property(c => c.CreatedAt)
            .HasDefaultValueSql("now()")
            .ValueGeneratedOnAdd();

        builder.Property(c => c.UserId)
            .IsRequired();

        builder.HasOne(c => c.Discipline)
            .WithMany(d => d.Classes)
            .HasForeignKey(c => c.DisciplineId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(c => c.User)
            .WithMany(u => u.Classes)
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}