using HiClass.Domain.Entities.Main;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HiClass.Persistence.EntityConfiguration.Main;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(t => t.UserId);
        builder.HasIndex(t => t.UserId)
            .IsUnique();

        builder.HasIndex(t => t.Email)
            .IsUnique();

        builder.Property(u => u.IsCreatedAccount)
            .HasDefaultValue(false)
            .IsRequired();

        builder.Property(u => u.VerificationCode)
            .HasMaxLength(10);

        builder.Property(u => u.IsVerified)
            .HasDefaultValue(false)
            .IsRequired();

        builder.Property(u => u.IsInstitutionVerified)
            .HasDefaultValue(false)
            .IsRequired();

        builder.Property(t => t.FirstName)
            .HasMaxLength(40);
        builder.Property(t => t.LastName)
            .HasMaxLength(40);

        builder.HasOne(t => t.Country)
            .WithMany(cl => cl.Users)
            .HasForeignKey(t => t.CountryId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(t => t.City)
            .WithMany(cl => cl.Users)
            .HasForeignKey(t => t.CityId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(t => t.Institution)
            .WithMany(e => e.Users)
            .HasForeignKey(t => t.InstitutionId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.Property(u => u.Rating)
            .HasColumnType("numeric(3,2)")
            .HasDefaultValue(0.0)
            .ValueGeneratedOnAdd();

        builder.Property(t => t.Description)
            .HasMaxLength(300);

        builder.Property(t => t.ImageUrl)
            .HasMaxLength(255);
        builder.Property(t => t.BannerImageUrl)
            .HasMaxLength(255);

        builder.Property(t => t.RegisteredAt)
            .HasDefaultValueSql("now()")
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder.Property(t => t.LastOnlineAt)
            .HasDefaultValueSql("now()");
    }
}