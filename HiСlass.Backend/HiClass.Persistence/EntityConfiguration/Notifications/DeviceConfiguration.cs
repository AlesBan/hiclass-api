using HiClass.Domain.Entities.Notifications;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HiClass.Persistence.EntityConfiguration.Notifications;

public class DeviceConfiguration : IEntityTypeConfiguration<Device>
{
    public void Configure(EntityTypeBuilder<Device> builder)
    {
        builder.HasKey(x => x.DeviceId);
        builder.HasIndex(x => x.DeviceId).IsUnique();
        builder.Property(x => x.DeviceId)
            .HasDefaultValueSql("gen_random_uuid()")
            .ValueGeneratedOnAdd();

        builder.HasKey(x => x.DeviceToken);
        builder.HasIndex(x => x.DeviceToken).IsUnique();
        builder.Property(x => x.DeviceToken).IsRequired();


        builder
            .HasOne(x => x.User)
            .WithMany(x => x.Devices)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}