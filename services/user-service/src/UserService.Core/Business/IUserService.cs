// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IUserService.cs" company="Electronic-Paradise">
//   © Electronic-Paradise. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using UserService.Abstraction.DTOs;

namespace UserService.Core.Business;

/// <summary>
/// Defines the contract for user profile business logic operations.
/// </summary>
public interface IUserService
{
    /// <summary>
    /// Retrieves a user profile by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the profile.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the user profile if found, otherwise null.</returns>
    Task<UserProfileDto?> GetByIdAsync(Guid id);

    /// <summary>
    /// Retrieves a user profile by the authentication service user ID.
    /// </summary>
    /// <param name="userId">The user ID from the authentication service.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the user profile if found, otherwise null.</returns>
    Task<UserProfileDto?> GetByUserIdAsync(Guid userId);

    /// <summary>
    /// Checks whether a phone number is already registered.
    /// </summary>
    /// <param name="phoneNumber">The phone number to check.</param>
    /// <returns>A task that represents the asynchronous operation. The task result indicates whether the phone number exists.</returns>
    Task<bool> PhoneNumberExistsAsync(string phoneNumber);

    /// <summary>
    /// Creates a new user profile.
    /// </summary>
    /// <param name="dto">The user creation data transfer object.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the created user profile.</returns>
    Task<UserProfileDto> CreateAsync(CreateUserDto dto);

    /// <summary>
    /// Updates an existing user profile.
    /// </summary>
    /// <param name="id">The unique identifier of the profile to update.</param>
    /// <param name="dto">The user update data transfer object.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the updated user profile if found, otherwise null.</returns>
    Task<UserProfileDto?> UpdateAsync(Guid id, CreateUserDto dto);

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
}
