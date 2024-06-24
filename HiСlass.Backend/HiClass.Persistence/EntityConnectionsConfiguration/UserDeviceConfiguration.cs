// using HiClass.Domain.EntityConnections;
// using Microsoft.EntityFrameworkCore;
// using Microsoft.EntityFrameworkCore.Metadata.Builders;
//
// namespace HiClass.Persistence.EntityConnectionsConfiguration;
//
// public class UserDeviceConfiguration : IEntityTypeConfiguration<UserDevice>
// {
//     public void Configure(EntityTypeBuilder<UserDevice> builder)
//     {
//         builder.HasKey(x => new { x.UserId, x.DeviceToken });
//         builder.HasIndex(x => new { x.UserId, x.DeviceToken })
//             .IsUnique();
//         builder.HasOne(x => x.User)
//             .WithMany(x => x.UserDevices)
//             .HasForeignKey(x => x.UserId)
//             .OnDelete(DeleteBehavior.Cascade);
//         builder.HasOne(x => x.Device)
//             .WithMany(x => x.UserDevices)
//             .HasForeignKey(x => x.DeviceToken)
//             .OnDelete(DeleteBehavior.Cascade);
//         
//         builder.Property(x => x.IsActive).IsRequired();
//         builder.Property(x => x.LastActive).IsRequired();
//     }
// }