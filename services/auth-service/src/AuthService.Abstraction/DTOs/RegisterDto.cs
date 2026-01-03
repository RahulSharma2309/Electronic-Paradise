namespace AuthService.Abstraction.DTOs;

public record RegisterDto(
    string Email,
    string Password,
    string ConfirmPassword,
    string? FullName,
    string? PhoneNumber,
    string? Address
);





