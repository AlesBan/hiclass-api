using HiClass.Domain.Entities.Communication;
using HiClass.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HiClass.Persistence.EntityConfiguration.Communication;

public class InvitationConfiguration : IEntityTypeConfiguration<Invitation>
{
    public void Configure(EntityTypeBuilder<Invitation> builder)
    {
        builder.HasKey(i => i.InvitationId);
        builder.HasIndex(i => i.InvitationId)
            .IsUnique();
        builder.Property(i => i.InvitationId)
            .HasDefaultValueSql("gen_random_uuid()")
            .ValueGeneratedOnAdd();

        builder.HasOne(i => i.UserSender)
            .WithMany(ti => ti.SentInvitations)
            .HasForeignKey(ti => ti.UserSenderId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

        builder.HasOne(i => i.UserRecipient)
            .WithMany(ur => ur.ReceivedInvitations)
            .HasForeignKey(i => i.UserRecipientId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

        builder.HasOne(i => i.ClassSender)
            .WithMany(cl => cl.SentInvitations)
            .HasForeignKey(i => i.ClassSenderId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

        builder.HasOne(i => i.ClassRecipient)
            .WithMany(cl => cl.ReceivedInvitations)
            .HasForeignKey(i => i.ClassRecipientId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(i => i.CreatedAt)
            .HasDefaultValueSql("now()")
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder.Property(i => i.DateOfInvitation)
            .IsRequired();

        builder.Property(i => i.InvitationText)
            .HasMaxLength(255);

        builder.Property(i => i.Status)
            .HasConversion(x => x.ToString(),
                x => (InvitationStatus)Enum.Parse(typeof(InvitationStatus), x))
            .IsRequired();
    }
}