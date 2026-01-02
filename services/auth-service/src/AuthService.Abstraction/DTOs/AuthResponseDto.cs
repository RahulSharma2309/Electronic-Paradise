namespace AuthService.Abstraction.DTOs;

public class AuthResponseDto
{
    public string Token { get; set; } = null!;
    public int ExpiresIn { get; set; }
    public Guid UserId { get; set; }
    public string Email { get; set; } = null!;
}

