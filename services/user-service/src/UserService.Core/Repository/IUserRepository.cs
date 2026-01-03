// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IUserRepository.cs" company="Electronic-Paradise">
//   © Electronic-Paradise. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using UserService.Abstraction.Models;

namespace UserService.Core.Repository;

/// <summary>
/// Defines the contract for user profile data access operations.
/// </summary>
public interface IUserRepository
{
    /// <summary>
    /// Retrieves a user profile by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the profile.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the user profile if found, otherwise null.</returns>
    Task<UserProfile?> GetByIdAsync(Guid id);

    /// <summary>
    /// Retrieves a user profile by the authentication service user ID.
    /// </summary>
    /// <param name="userId">The user ID from the authentication service.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the user profile if found, otherwise null.</returns>
    Task<UserProfile?> GetByUserIdAsync(Guid userId);

    /// <summary>
    /// Retrieves a user profile by phone number.
    /// </summary>
    /// <param name="phoneNumber">The phone number to search for.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the user profile if found, otherwise null.</returns>
    Task<UserProfile?> GetByPhoneNumberAsync(string phoneNumber);

    /// <summary>
    /// Creates a new user profile in the repository.
    /// </summary>
    /// <param name="profile">The user profile to create.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the created user profile.</returns>
    Task<UserProfile> CreateAsync(UserProfile profile);

    /// <summary>
    /// Updates an existing user profile in the repository.
    /// </summary>
    /// <param name="profile">The user profile with updated information.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the updated user profile.</returns>
    Task<UserProfile> UpdateAsync(UserProfile profile);

    /// <summary>
    /// Debits (subtracts) an amount from a user's wallet balance.
    /// </summary>
    /// <param name="id">The unique identifier of the user profile.</param>
    /// <param name="amount">The amount to debit.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the new wallet balance.</returns>
    Task<decimal> DebitWalletAsync(Guid id, decimal amount);

    /// <summary>
    /// Credits (adds) an amount to a user's wallet balance.
    /// </summary>
    /// <param name="id">The unique identifier of the user profile.</param>
    /// <param name="amount">The amount to credit.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the new wallet balance.</returns>
    Task<decimal> CreditWalletAsync(Guid id, decimal amount);

    /// <summary>
    /// Retrieves all user profiles from the repository.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains a list of all user profiles.</returns>
    Task<List<UserProfile>> GetAllAsync();
}
