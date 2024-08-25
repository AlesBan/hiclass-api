using HiClass.Domain.Entities.Notifications;
using HiClass.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HiClass.Persistence.EntityConfiguration.Notifications;

public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
{
    public void Configure(EntityTypeBuilder<Notification> builder)
    {
        builder.HasKey(x => x.NotificationId);

        builder.HasIndex(x => x.NotificationId)
            .IsUnique();

        builder.Property(x => x.NotificationId)
            .HasDefaultValueSql("gen_random_uuid()")
            .ValueGeneratedOnAdd();

        builder
            .HasOne(x => x.UserReceiver)
            .WithMany(x => x.Notifications)
            .HasForeignKey(x => x.UserReceiverId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(c => c.CreatedAt)
            .HasDefaultValueSql("now()")
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder.Property(c => c.Status)
            .HasConversion(
                x => x.ToString(),
                x => (NotificationStatus)Enum.Parse(typeof(NotificationStatus), x))
            .IsRequired();

        builder.Property(c => c.Type)
            .HasConversion(
                x => x.ToString(),
                x => (NotificationType)Enum.Parse(typeof(NotificationType), x))
            .IsRequired();

        builder.Property(c => c.Message)
            .HasMaxLength(255);

        builder.Property(c => c.IsDeleted)
            .HasDefaultValue(false);
    }
}