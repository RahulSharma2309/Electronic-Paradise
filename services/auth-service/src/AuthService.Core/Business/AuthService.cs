using AuthService.Abstraction.DTOs;
using AuthService.Abstraction.Models;
using AuthService.Core.Repository;
using Ep.Platform.Security;

namespace AuthService.Core.Business;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;

    public AuthService(IUserRepository userRepository, IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task<User> RegisterAsync(RegisterDto dto)
    {
        // Hash password using Platform service
        var passwordHash = _passwordHasher.HashPassword(dto.Password);

        var user = new User
        {
            Email = dto.Email,
            PasswordHash = passwordHash,
            FullName = dto.FullName
        };

        await _userRepository.AddAsync(user);
        return user;
    }

    public async Task<(User? user, string? error)> LoginAsync(LoginDto dto)
    {
        var user = await _userRepository.FindByEmailAsync(dto.Email);
        if (user == null)
        {
            return (null, "Invalid credentials");
        }

        // Verify password using Platform service
        var valid = _passwordHasher.VerifyPassword(dto.Password, user.PasswordHash);
        if (!valid)
        {
            return (null, "Invalid credentials");
        }

        return (user, null);
    }

    public async Task<bool> ResetPasswordAsync(ResetPasswordDto dto)
    {
        var user = await _userRepository.FindByEmailAsync(dto.Email);
        if (user == null)
        {
            return false;
        }

        user.PasswordHash = _passwordHasher.HashPassword(dto.NewPassword);
        await _userRepository.UpdateAsync(user);
        return true;
    }

    public async Task<User?> GetUserByIdAsync(Guid id)
    {
        return await _userRepository.FindByIdAsync(id);
    }
}
