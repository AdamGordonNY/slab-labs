using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SlabLabs.Api.Models;

namespace SlabLabs.Api.Data;

public class PasswordResetTokenConfiguration : IEntityTypeConfiguration<PasswordResetToken>
{
    public void Configure(EntityTypeBuilder<PasswordResetToken> builder)
    {
        builder.ToTable("password_reset_tokens");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Token)
               .IsRequired()
               .HasMaxLength(512);

        builder.Property(p => p.RequestedFromIp)
               .HasMaxLength(45);

        // ✅ Relationship
        builder.HasOne(p => p.User)
               .WithMany(u => u.PasswordResetTokens)
               .HasForeignKey(p => p.UserId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}