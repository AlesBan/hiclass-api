using HiClass.Domain.EntityConnections;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HiClass.Persistence.EntityConnectionsConfiguration;

public class UserDeviceConfiguration : IEntityTypeConfiguration<UserDevice>
{
    public void Configure(EntityTypeBuilder<UserDevice> builder)
    {
        builder.HasKey(x => new { x.UserId, x.DeviceId });
        builder.HasIndex(x => new { x.UserId, x.DeviceId })
            .IsUnique();
        
        builder.HasOne(x => x.User)
            .WithMany(x => x.UserDevices)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasOne(x => x.Device)
            .WithMany(x => x.UserDevices)
            .HasForeignKey(x => x.DeviceId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(x => x.IsActive).IsRequired();
    }
}