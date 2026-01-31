using UserService.Abstraction.DTOs.Requests;
using UserService.Abstraction.DTOs.Responses;
using UserService.Abstraction.Models;

namespace UserService.Core.Mappers;

/// <summary>
/// Defines the contract for mapping between UserProfile domain models and DTOs.
/// </summary>
public interface IUserProfileMapper
{
    /// <summary>
    /// Maps a UserProfile domain model to a lightweight UserProfileResponse DTO.
    /// </summary>
    /// <param name="profile">The user profile domain model.</param>
    /// <returns>The user profile response DTO.</returns>
    UserProfileResponse ToResponse(UserProfile profile);

    /// <summary>
    /// Maps a UserProfile domain model to a comprehensive UserProfileDetailResponse DTO.
    /// </summary>
    /// <param name="profile">The user profile domain model.</param>
    /// <returns>The user profile detail response DTO.</returns>
    UserProfileDetailResponse ToDetailResponse(UserProfile profile);

    /// <summary>
    /// Maps a CreateUserProfileRequest DTO to a UserProfile domain model.
    /// </summary>
    /// <param name="request">The create user profile request DTO.</param>
    /// <returns>The user profile domain model.</returns>
    UserProfile ToEntity(CreateUserProfileRequest request);

    /// <summary>
    /// Updates an existing UserProfile domain model with data from an UpdateUserProfileRequest DTO.
    /// Only updates fields that are provided (not null).
    /// </summary>
    /// <param name="profile">The existing user profile domain model to update.</param>
    /// <param name="request">The update user profile request DTO.</param>
    void UpdateEntity(UserProfile profile, UpdateUserProfileRequest request);

    /// <summary>
    /// Maps wallet balance information to a WalletBalanceResponse DTO.
    /// </summary>
    /// <param name="userId">The user ID.</param>
    /// <param name="balance">The wallet balance.</param>
    /// <param name="message">Optional message.</param>
    /// <returns>The wallet balance response DTO.</returns>
    WalletBalanceResponse ToWalletBalanceResponse(Guid userId, decimal balance, string? message = null);
}
