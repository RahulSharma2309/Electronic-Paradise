using AuthService.Abstraction.DTOs.Requests;
using AuthService.Abstraction.DTOs.Responses;
using AuthService.Abstraction.Models;

namespace AuthService.Core.Mappers;

/// <summary>
/// Defines the contract for mapping between User domain models and DTOs.
/// </summary>
public interface IAuthMapper
{
    /// <summary>
    /// Maps a User domain model to a lightweight UserResponse DTO.
    /// </summary>
    /// <param name="user">The user domain model.</param>
    /// <returns>The user response DTO.</returns>
    UserResponse ToResponse(User user);

    /// <summary>
    /// Maps a User domain model to a comprehensive UserDetailResponse DTO.
    /// </summary>
    /// <param name="user">The user domain model.</param>
    /// <returns>The user detail response DTO.</returns>
    UserDetailResponse ToDetailResponse(User user);

    /// <summary>
    /// Maps a User domain model and JWT token to an AuthResponse DTO.
    /// </summary>
    /// <param name="user">The user domain model.</param>
    /// <param name="token">The JWT access token.</param>
    /// <param name="expiresAt">The token expiration timestamp.</param>
    /// <returns>The auth response DTO.</returns>
    AuthResponse ToAuthResponse(User user, string token, DateTime expiresAt);

    /// <summary>
    /// Maps a RegisterRequest DTO to a User domain model.
    /// </summary>
    /// <param name="request">The register request DTO.</param>
    /// <param name="hashedPassword">The hashed password.</param>
    /// <returns>The user domain model.</returns>
    User ToEntity(RegisterRequest request, string hashedPassword);
}
