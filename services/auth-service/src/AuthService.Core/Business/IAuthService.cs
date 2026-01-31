// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IAuthService.cs" company="Electronic-Paradise">
//   © Electronic-Paradise. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using AuthService.Abstraction.DTOs.Requests;
using AuthService.Abstraction.DTOs.Responses;

namespace AuthService.Core.Business;

/// <summary>
/// Defines the contract for authentication business logic operations.
/// </summary>
public interface IAuthService
{
    /// <summary>
    /// Registers a new user account in the system.
    /// </summary>
    /// <param name="request">The registration request containing user information.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the authentication response with token and user info.</returns>
    Task<AuthResponse> RegisterAsync(RegisterRequest request);

    /// <summary>
    /// Authenticates a user with their credentials.
    /// </summary>
    /// <param name="request">The login request containing email and password.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the authentication response with token and user info.</returns>
    /// <exception cref="UnauthorizedAccessException">Thrown if credentials are invalid.</exception>
    Task<AuthResponse> LoginAsync(LoginRequest request);

    /// <summary>
    /// Resets a user's password.
    /// </summary>
    /// <param name="request">The reset password request containing email, token, and new password.</param>
    /// <returns>A task that represents the asynchronous operation. The task result indicates whether the operation succeeded.</returns>
    Task<bool> ResetPasswordAsync(ResetPasswordRequest request);

    /// <summary>
    /// Updates user information.
    /// </summary>
    /// <param name="id">The unique identifier of the user.</param>
    /// <param name="request">The update user request.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the updated user details if found, otherwise null.</returns>
    Task<UserDetailResponse?> UpdateUserAsync(Guid id, UpdateUserRequest request);

    /// <summary>
    /// Retrieves a user by their unique identifier (comprehensive details).
    /// </summary>
    /// <param name="id">The unique identifier of the user.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the user if found, otherwise null.</returns>
    Task<UserDetailResponse?> GetUserByIdAsync(Guid id);

    /// <summary>
    /// Retrieves a user by their email address (lightweight).
    /// </summary>
    /// <param name="email">The email address of the user.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the user if found, otherwise null.</returns>
    Task<UserResponse?> GetUserByEmailAsync(string email);
}
