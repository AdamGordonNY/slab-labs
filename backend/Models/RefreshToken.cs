
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Slablabs.Api.Models;

public class RefreshToken
{
    public int Id { get; set; }

    [Required]
    public int UserId { get; set; }

    public ApplicationUser User { get; set; } = null!;

    [Required]
    [MaxLength(512)]
    public string Token { get; set; } = string.Empty;

    public DateTime ExpiresAt { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? RevokedAt { get; set; }        // Null = still active

    [MaxLength(45)]
    public string? CreatedByIp { get; set; }        // For audit/security

    [MaxLength(45)]
    public string? RevokedByIp { get; set; }

    [MaxLength(512)]
    public string? ReplacedByToken { get; set; }    // Token rotation tracking



    // Computed helpers (not mapped)
    [NotMapped]
    public bool IsExpired => DateTime.UtcNow >= ExpiresAt;

    [NotMapped]
    public bool IsRevoked => RevokedAt != null;

    [NotMapped]
    public bool IsActive => !IsExpired && !IsRevoked;

    public static implicit operator string(RefreshToken v)
    {
        throw new NotImplementedException();
    }
}
