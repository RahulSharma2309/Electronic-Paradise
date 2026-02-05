// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IUserService.cs" company="FreshHarvest-Market">
//   © FreshHarvest-Market. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using UserService.Abstraction.DTOs.Requests;
using UserService.Abstraction.DTOs.Responses;

namespace UserService.Core.Business;

/// <summary>
/// Defines the contract for user profile business logic operations.
/// </summary>
public interface IUserService
{
    /// <summary>
    /// Retrieves a user profile by its unique identifier (comprehensive details).
    /// </summary>
    /// <param name="id">The unique identifier of the profile.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the user profile if found, otherwise null.</returns>
    Task<UserProfileDetailResponse?> GetByIdAsync(Guid id);

    /// <summary>
    /// Retrieves a user profile by the authentication service user ID (lightweight).
    /// </summary>
    /// <param name="userId">The user ID from the authentication service.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the user profile if found, otherwise null.</returns>
    Task<UserProfileResponse?> GetByUserIdAsync(Guid userId);

    /// <summary>
    /// Checks whether a phone number is already registered.
    /// </summary>
    /// <param name="phoneNumber">The phone number to check.</param>
    /// <returns>A task that represents the asynchronous operation. The task result indicates whether the phone number exists.</returns>
    Task<bool> PhoneNumberExistsAsync(string phoneNumber);

    /// <summary>
    /// Creates a new user profile.
    /// </summary>
    /// <param name="request">The user creation request.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the created user profile.</returns>
    Task<UserProfileDetailResponse> CreateAsync(CreateUserProfileRequest request);

    /// <summary>
    /// Updates an existing user profile.
    /// </summary>
    /// <param name="id">The unique identifier of the profile to update.</param>
    /// <param name="request">The user update request.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the updated user profile if found, otherwise null.</returns>
    Task<UserProfileDetailResponse?> UpdateAsync(Guid id, UpdateUserProfileRequest request);

    /// <summary>
    /// Adds balance to a user's wallet.
    /// </summary>
    /// <param name="userId">The user ID from the authentication service.</param>
    /// <param name="request">The add balance request.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the wallet balance response.</returns>
    Task<WalletBalanceResponse> AddBalanceAsync(Guid userId, AddBalanceRequest request);

    /// <summary>
    /// Debits (subtracts) an amount from a user's wallet balance.
    /// </summary>
    /// <param name="userId">The user ID from the authentication service.</param>
    /// <param name="amount">The amount to debit.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the wallet balance response.</returns>
    Task<WalletBalanceResponse> DebitWalletAsync(Guid userId, decimal amount);

    /// <summary>
    /// Credits (adds) an amount to a user's wallet balance.
    /// </summary>
    /// <param name="userId">The user ID from the authentication service.</param>
    /// <param name="amount">The amount to credit.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the wallet balance response.</returns>
    Task<WalletBalanceResponse> CreditWalletAsync(Guid userId, decimal amount);
}
