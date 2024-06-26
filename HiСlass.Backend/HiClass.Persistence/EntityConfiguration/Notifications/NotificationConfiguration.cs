using HiClass.Domain.Entities.Notifications;
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

        builder.Property(c => c.IsRead)
            .HasDefaultValue(false)
            .IsRequired();

        builder.Property(c => c.NotificationType)
            .HasMaxLength(40)
            .IsRequired();
    }
}