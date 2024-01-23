using HiClass.Domain.Entities.Communication;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HiClass.Persistence.EntityConfiguration.Communication;

public class FeedbackConfiguration : IEntityTypeConfiguration<Feedback>
{
    public void Configure(EntityTypeBuilder<Feedback> builder)
    {
        builder.HasKey(f => f.FeedbackId);
        builder.HasIndex(f => f.FeedbackId)
            .IsUnique();
        builder.Property(c => c.FeedbackId)
            .HasDefaultValueSql("gen_random_uuid()")
            .ValueGeneratedOnAdd();

        builder.HasOne(f => f.Invitation)
            .WithMany(i => i.Feedbacks)
            .HasForeignKey(f => f.InvitationId)
            .IsRequired();

        builder.HasOne(f => f.UserSender)
            .WithMany(u => u.SentFeedbacks)
            .HasForeignKey(f => f.UserSenderId)
            .IsRequired();
        
        builder.HasOne(f => f.UserRecipient)
            .WithMany(u => u.ReceivedFeedbacks)
            .HasForeignKey(f => f.UserRecipientId)
            .IsRequired();
        
        builder.Property(f => f.WasTheJointLesson)
            .IsRequired();

        builder.Property(f => f.ReasonForNotConducting)
            .HasMaxLength(255);

        builder.Property(f => f.FeedbackText)
            .HasMaxLength(255);
        builder.Property(f => f.Rating)
            .HasColumnType("SMALLINT");
    }
}