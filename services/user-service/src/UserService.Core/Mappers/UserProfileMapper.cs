using UserService.Abstraction.DTOs.Requests;
using UserService.Abstraction.DTOs.Responses;
using UserService.Abstraction.Models;

namespace UserService.Core.Mappers;

/// <summary>
/// Implementation of user profile mapping between domain models and DTOs.
/// </summary>
public class UserProfileMapper : IUserProfileMapper
{
    /// <inheritdoc/>
    public UserProfileResponse ToResponse(UserProfile profile)
    {
        return new UserProfileResponse
        {
            Id = profile.Id,
            UserId = profile.UserId,
            FirstName = profile.FirstName,
            LastName = profile.LastName,
            Address = profile.Address,
            PhoneNumber = profile.PhoneNumber,
            WalletBalance = profile.WalletBalance,
        };
    }

    /// <inheritdoc/>
    public UserProfileDetailResponse ToDetailResponse(UserProfile profile)
    {
        return new UserProfileDetailResponse
        {
            Id = profile.Id,
            UserId = profile.UserId,
            FirstName = profile.FirstName,
            LastName = profile.LastName,
            Address = profile.Address,
            PhoneNumber = profile.PhoneNumber,
            WalletBalance = profile.WalletBalance,
            CreatedAt = profile.CreatedAt,
            UpdatedAt = profile.UpdatedAt,
        };
    }

    /// <inheritdoc/>
    public UserProfile ToEntity(CreateUserProfileRequest request)
    {
        return new UserProfile
        {
            UserId = request.UserId,
            FirstName = request.FirstName,
            LastName = request.LastName,
            Address = request.Address,
            PhoneNumber = request.PhoneNumber,
            WalletBalance = 0,
            CreatedAt = DateTime.UtcNow,
        };
    }

    /// <inheritdoc/>
    public void UpdateEntity(UserProfile profile, UpdateUserProfileRequest request)
    {
        // Update only provided fields (PATCH semantics)
        if (request.FirstName != null)
        {
            profile.FirstName = request.FirstName;
        }

        if (request.LastName != null)
        {
            profile.LastName = request.LastName;
        }

        if (request.Address != null)
        {
            profile.Address = request.Address;
        }

        if (request.PhoneNumber != null)
        {
            profile.PhoneNumber = request.PhoneNumber;
        }

        profile.UpdatedAt = DateTime.UtcNow;
    }

    /// <inheritdoc/>
    public WalletBalanceResponse ToWalletBalanceResponse(Guid userId, decimal balance, string? message = null)
    {
        return new WalletBalanceResponse
        {
            UserId = userId,
            Balance = balance,
            Timestamp = DateTime.UtcNow,
            Message = message ?? $"Current balance: ${balance:F2}",
        };
    }
}
