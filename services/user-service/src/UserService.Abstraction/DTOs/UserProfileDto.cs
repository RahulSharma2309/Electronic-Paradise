using UserService.Abstraction.Models;

namespace UserService.Abstraction.DTOs;

public class UserProfileDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Address { get; set; }
    public string? PhoneNumber { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public decimal WalletBalance { get; set; }

    public static UserProfileDto FromModel(UserProfile m) => new UserProfileDto
    {
        Id = m.Id,
        UserId = m.UserId,
        FirstName = m.FirstName,
        LastName = m.LastName,
        Address = m.Address,
        PhoneNumber = m.PhoneNumber,
        CreatedAt = m.CreatedAt,
        UpdatedAt = m.UpdatedAt,
        WalletBalance = m.WalletBalance
    };
}




