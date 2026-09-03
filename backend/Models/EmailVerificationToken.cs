
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;



namespace Slablabs.Api.Models;

public class EmailVerificationToken
{
    public int Id { get; set; }

    [Required]
    public int UserId { get; set; }
    public ApplicationUser User { get; set; } = null!;
    [Required]
    [MaxLength(512)]
    public string Token { get; set; } = string.Empty;

    public DateTime ExpiresAt { get; set; }             // Typically 24�48 hours

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? VerifiedAt { get; set; }           // Null = unverified



    [NotMapped]
    public bool IsExpired => DateTime.UtcNow >= ExpiresAt;

    [NotMapped]
    public bool IsVerified => VerifiedAt != null;

    [NotMapped]
    public bool IsValid => !IsExpired && !IsVerified;
}
