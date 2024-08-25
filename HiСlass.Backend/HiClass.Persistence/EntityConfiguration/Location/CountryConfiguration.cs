using HiClass.Domain.Entities.Location;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HiClass.Persistence.EntityConfiguration.Location;

public class CountryConfiguration : IEntityTypeConfiguration<Country>
{
    public void Configure(EntityTypeBuilder<Country> builder)
    {
        builder.HasKey(c => c.CountryId);

        builder.HasIndex(c => c.CountryId)
            .IsUnique();

        builder.HasIndex(c => c.Title)
            .IsUnique();

        builder.Property(c => c.CountryId)
            .HasDefaultValueSql("gen_random_uuid()")
            .ValueGeneratedOnAdd();

        builder.Property(c => c.Title)
            .HasMaxLength(50)
            .IsRequired();
    }
}