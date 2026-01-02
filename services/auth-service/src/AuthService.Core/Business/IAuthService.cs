using AuthService.Abstraction.DTOs;
using AuthService.Abstraction.Models;

namespace AuthService.Core.Business;

public interface IAuthService
{
    Task<User> RegisterAsync(RegisterDto dto);
    Task<(User? user, string? error)> LoginAsync(LoginDto dto);
    Task<bool> ResetPasswordAsync(ResetPasswordDto dto);
    Task<User?> GetUserByIdAsync(Guid id);
}

