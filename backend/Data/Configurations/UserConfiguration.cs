// Data/Configurations/UserConfiguration.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace SlabsLabs.Api.Models.User;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
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
