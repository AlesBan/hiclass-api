using HiClass.Domain.Entities.Education;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HiClass.Persistence.EntityConfiguration.Education;

public class GradeConfiguration: IEntityTypeConfiguration<Grade>
{
    public void Configure(EntityTypeBuilder<Grade> builder)
    {
        builder.HasKey(g => g.GradeId);
        builder.HasIndex(g => g.GradeId)
            .IsUnique();
        builder.Property(g => g.GradeId)
            .HasDefaultValueSql("gen_random_uuid()")
            .ValueGeneratedOnAdd();

        builder.Property(g => g.GradeNumber)
            .HasMaxLength(15);
    }
}