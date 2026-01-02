using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UserService.Abstraction.Models;

public class UserProfile
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    // This should map to the identity/user id in Auth service (not a password or credential)
    [Required]
    public Guid UserId { get; set; }

    [MaxLength(100)]
    public string? FirstName { get; set; }

    [MaxLength(100)]
    public string? LastName { get; set; }

    [MaxLength(500)]
    public string? Address { get; set; }

    [MaxLength(50)]
    public string? PhoneNumber { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Wallet balance for e-commerce transactions
    [Column(TypeName = "decimal(18,2)")]
    public decimal WalletBalance { get; set; } = 0;
}




