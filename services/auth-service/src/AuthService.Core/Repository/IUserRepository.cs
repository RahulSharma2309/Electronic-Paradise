// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IUserRepository.cs" company="Electronic-Paradise">
//   © Electronic-Paradise. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using AuthService.Abstraction.Models;

namespace AuthService.Core.Repository;

/// <summary>
/// Defines the contract for user data access operations.
/// </summary>
public interface IUserRepository
{
    /// <summary>
    /// Adds a new user to the repository.
    /// </summary>
    /// <param name="user">The user entity to add.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task AddAsync(User user);

    /// <summary>
    /// Finds a user by their email address.
    /// </summary>
    /// <param name="email">The email address to search for.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the user if found, otherwise null.</returns>
    Task<User?> FindByEmailAsync(string email);

    /// <summary>
    /// Finds a user by their unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the user.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the user if found, otherwise null.</returns>
    Task<User?> FindByIdAsync(Guid id);

    /// <summary>
    /// Checks whether an email address is already registered.
    /// </summary>
    /// <param name="email">The email address to check.</param>
    /// <returns>A task that represents the asynchronous operation. The task result indicates whether the email exists.</returns>
    Task<bool> EmailExistsAsync(string email);

    /// <summary>
    /// Updates an existing user in the repository.
    /// </summary>
    /// <param name="user">The user entity with updated information.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task UpdateAsync(User user);

    /// <summary>
    /// Deletes a user by their unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the user to delete.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task DeleteAsync(Guid id);

    /// <summary>
    /// Persists all pending changes to the data store.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task SaveChangesAsync();
}
