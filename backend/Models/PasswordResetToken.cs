
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Slablabs.Api.Models;

public class PasswordResetToken
{
    public int Id { get; set; }

    [Required]
    public int UserId { get; set; }
    public ApplicationUser User { get; set; } = null!;
    [Required]
    [MaxLength(512)]
    public string Token { get; set; } = string.Empty;  // Store hashed

    public DateTime ExpiresAt { get; set; }             // Typically 15�60 mins

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UsedAt { get; set; }               // Null = not yet used

    [MaxLength(45)]
    public string? RequestedFromIp { get; set; }


    [NotMapped]
    public bool IsExpired => DateTime.UtcNow >= ExpiresAt;

    [NotMapped]
    public bool IsUsed => UsedAt != null;

    [NotMapped]
    public bool IsValid => !IsExpired && !IsUsed;
}
