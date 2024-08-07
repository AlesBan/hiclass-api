using HiClass.Domain.EntityConnections;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HiClass.Persistence.EntityConnectionsConfiguration;

public class ClassLanguageConfiguration : IEntityTypeConfiguration<ClassLanguage>
{
    public void Configure(EntityTypeBuilder<ClassLanguage> builder)
    {
        builder.HasKey(cl => new { cl.LanguageId, cl.ClassId });
        builder.HasIndex(cl => new { cl.LanguageId, cl.ClassId })
            .IsUnique();

        builder.HasOne(cl => cl.Class)
            .WithMany(c => c.ClassLanguages)
            .HasForeignKey(cl => cl.ClassId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasOne(cl => cl.Language)
            .WithMany(c => c.ClassLanguages)
            .HasForeignKey(cl => cl.LanguageId)            
            .OnDelete(DeleteBehavior.Cascade);
    }
}