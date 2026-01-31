using AuthService.Abstraction.DTOs.Requests;
using AuthService.Abstraction.DTOs.Responses;
using AuthService.Abstraction.Models;

namespace AuthService.Core.Mappers;

/// <summary>
/// Implementation of auth mapping between domain models and DTOs.
/// </summary>
public class AuthMapper : IAuthMapper
{
    /// <inheritdoc/>
    public UserResponse ToResponse(User user)
    {
        return new UserResponse
        {
            Id = user.Id,
            Email = user.Email ?? string.Empty,
            FullName = user.FullName ?? string.Empty,
            CreatedAt = user.CreatedAt,
        };
    }

    /// <inheritdoc/>
    public UserDetailResponse ToDetailResponse(User user)
    {
        return new UserDetailResponse
        {
            Id = user.Id,
            Email = user.Email ?? string.Empty,
            FullName = user.FullName ?? string.Empty,
            CreatedAt = user.CreatedAt,
            LastLoginAt = null, // Can be extended in future
            IsEmailVerified = false, // Can be extended in future
            IsActive = true,
        };
    }

    /// <inheritdoc/>
    public AuthResponse ToAuthResponse(User user, string token, DateTime expiresAt)
    {
        return new AuthResponse
        {
            Token = token,
            ExpiresAt = expiresAt,
            User = ToResponse(user),
        };
    }

    /// <inheritdoc/>
    public User ToEntity(RegisterRequest request, string hashedPassword)
    {
        return new User
        {
            Email = request.Email,
            PasswordHash = hashedPassword,
            FullName = request.FullName,
            CreatedAt = DateTime.UtcNow,
        };
    }
}
