using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SlabLabs.Api.Models;

namespace SlabLabs.Api.Data;

public class UserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
       public void Configure(EntityTypeBuilder<ApplicationUser> builder)
       {
              builder.ToTable("users");

              builder.HasKey(u => u.Id);

              builder.HasIndex(u => u.Email).IsUnique();

              builder.Property(u => u.Email)
                     .IsRequired()
                     .HasMaxLength(255);

              builder.Property(u => u.PasswordHash)
                     .IsRequired()
                     .HasMaxLength(255);

              builder.Property(u => u.Role)
                     .HasConversion<string>()
                     .HasMaxLength(50);

              builder.Property(u => u.Status)
                     .HasConversion<string>()
                     .HasMaxLength(50);
       }
}
