// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AuthService.cs" company="FreshHarvest-Market">
//   © FreshHarvest-Market. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Net;

using AuthService.Abstraction.DTOs.Requests;
using AuthService.Abstraction.DTOs.Responses;
using AuthService.Abstraction.Models;
using AuthService.Core.Clients;
using AuthService.Core.Clients.Models;
using AuthService.Core.Mappers;
using AuthService.Core.Repository;

using Ep.Platform.Security;
using Microsoft.Extensions.Logging;

namespace AuthService.Core.Business;

/// <summary>
/// Implements authentication business logic operations.
/// </summary>
public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IUserServiceClient _userServiceClient;
    private readonly IAuthMapper _mapper;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenGenerator _jwtGenerator;
    private readonly ILogger<AuthService> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="AuthService"/> class.
    /// </summary>
    /// <param name="userRepository">
    /// The user repository for data access.
    /// </param>
    /// <param name="userServiceClient">
    /// The user service client for external user profile operations.
    /// </param>
    /// <param name="mapper">
    /// The auth mapper.
    /// </param>
    /// <param name="passwordHasher">
    /// The password hasher for secure password handling.
    /// </param>
    /// <param name="jwtGenerator">
    /// The JWT token generator.
    /// </param>
    /// <param name="logger">
    /// The logger instance for structured logging.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when any parameter is null.
    /// </exception>
    public AuthService(
        IUserRepository userRepository,
        IUserServiceClient userServiceClient,
        IAuthMapper mapper,
        IPasswordHasher passwordHasher,
        IJwtTokenGenerator jwtGenerator,
        ILogger<AuthService> logger)
    {
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        _userServiceClient = userServiceClient ?? throw new ArgumentNullException(nameof(userServiceClient));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
        _jwtGenerator = jwtGenerator ?? throw new ArgumentNullException(nameof(jwtGenerator));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc/>
    public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
    {
        _logger.LogInformation("Registering new user with email: {Email}", request.Email);

        // 1) Prevent duplicates (authdb email)
        var emailExists = await _userRepository.EmailExistsAsync(request.Email);
        if (emailExists)
        {
            throw new InvalidOperationException("EMAIL_EXISTS");
        }

        // 2) Prevent duplicates (userdb phone) - mandatory check
        try
        {
            var phoneExists = await _userServiceClient.PhoneExistsAsync(request.PhoneNumber);
            if (phoneExists)
            {
                throw new InvalidOperationException("PHONE_EXISTS");
            }
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.ServiceUnavailable)
        {
            throw;
        }

        // 3) Create auth user
        var passwordHash = _passwordHasher.HashPassword(request.Password);
        var user = new User
        {
            Email = request.Email,
            PasswordHash = passwordHash,
            FullName = request.FullName,
        };

        await _userRepository.AddAsync(user);

        // 4) Create user profile (compensate by deleting auth user if it fails)
        try
        {
            var (firstName, lastName) = SplitName(request.FullName);

            await _userServiceClient.CreateUserProfileAsync(
                new CreateUserProfileRequest(
                    user.Id,
                    firstName,
                    lastName,
                    request.PhoneNumber,
                    request.Address));
        }
        catch
        {
            try
            {
                await _userRepository.DeleteAsync(user.Id);
            }
            catch (Exception rollbackEx)
            {
                _logger.LogError(rollbackEx, "Registration rollback failed for auth user {UserId}", user.Id);
            }

            throw;
        }

        // 5) Generate JWT token
        var claims = new Dictionary<string, string>
        {
            { "userId", user.Id.ToString() },
            { "email", user.Email },
            { "fullName", user.FullName ?? string.Empty },
        };
        var token = _jwtGenerator.GenerateToken(claims, TimeSpan.FromHours(24));
        var expiresAt = DateTime.UtcNow.AddHours(24);

        _logger.LogInformation("User registered successfully with ID: {UserId}, Email: {Email}", user.Id, user.Email);
        return _mapper.ToAuthResponse(user, token, expiresAt);
    }

    /// <inheritdoc/>
    public async Task<AuthResponse> LoginAsync(LoginRequest request)
    {
        try
        {
            _logger.LogInformation("Login attempt for email: {Email}", request.Email);

            var user = await _userRepository.FindByEmailAsync(request.Email);
            if (user == null)
            {
                _logger.LogWarning("Login failed: User not found for email: {Email}", request.Email);
                throw new KeyNotFoundException("USER_NOT_FOUND");
            }

            // Verify password using Platform service
            var valid = _passwordHasher.VerifyPassword(request.Password, user.PasswordHash);
            if (!valid)
            {
                _logger.LogWarning("Login failed: Invalid password for email: {Email}", request.Email);
                throw new UnauthorizedAccessException("Invalid credentials");
            }

            // Generate JWT token
            var claims = new Dictionary<string, string>
            {
                { "userId", user.Id.ToString() },
                { "email", user.Email },
                { "fullName", user.FullName ?? string.Empty },
            };
            var token = _jwtGenerator.GenerateToken(claims, TimeSpan.FromHours(24));
            var expiresAt = DateTime.UtcNow.AddHours(24);

            _logger.LogInformation("User logged in successfully: {UserId}, Email: {Email}", user.Id, user.Email);
            return _mapper.ToAuthResponse(user, token, expiresAt);
        }
        catch (UnauthorizedAccessException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during login attempt for email: {Email}", request.Email);
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<bool> ResetPasswordAsync(ResetPasswordRequest request)
    {
        try
        {
            _logger.LogInformation("Password reset attempt for email: {Email}", request.Email);

            var user = await _userRepository.FindByEmailAsync(request.Email);
            if (user == null)
            {
                _logger.LogWarning("Password reset failed: User not found for email: {Email}", request.Email);
                return false;
            }

            user.PasswordHash = _passwordHasher.HashPassword(request.NewPassword);
            await _userRepository.UpdateAsync(user);

            _logger.LogInformation("Password reset successful for user: {UserId}, Email: {Email}", user.Id, user.Email);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during password reset for email: {Email}", request.Email);
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<UserDetailResponse?> GetUserByIdAsync(Guid id)
    {
        try
        {
            _logger.LogDebug("Retrieving user by ID: {UserId}", id);

            var user = await _userRepository.FindByIdAsync(id);

            if (user == null)
            {
                _logger.LogWarning("User not found with ID: {UserId}", id);
                return null;
            }

            _logger.LogDebug("User retrieved successfully: {UserId}, Email: {Email}", user.Id, user.Email);
            return _mapper.ToDetailResponse(user);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving user by ID: {UserId}", id);
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<UserResponse?> GetUserByEmailAsync(string email)
    {
        try
        {
            _logger.LogDebug("Retrieving user by email: {Email}", email);

            var user = await _userRepository.FindByEmailAsync(email);

            if (user == null)
            {
                _logger.LogWarning("User not found with email: {Email}", email);
                return null;
            }

            _logger.LogDebug("User retrieved successfully: {UserId}, Email: {Email}", user.Id, user.Email);
            return _mapper.ToResponse(user);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving user by email: {Email}", email);
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<UserDetailResponse?> UpdateUserAsync(Guid id, UpdateUserRequest request)
    {
        try
        {
            _logger.LogInformation("Updating user {UserId}", id);

            var user = await _userRepository.FindByIdAsync(id);
            if (user == null)
            {
                _logger.LogWarning("Update failed: User {UserId} not found", id);
                return null;
            }

            // Update user properties
            if (!string.IsNullOrWhiteSpace(request.Email))
            {
                user.Email = request.Email;
            }

            if (!string.IsNullOrWhiteSpace(request.FullName))
            {
                user.FullName = request.FullName;
            }

            await _userRepository.UpdateAsync(user);

            _logger.LogInformation("User {UserId} updated successfully", id);
            return _mapper.ToDetailResponse(user);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating user {UserId}", id);
            throw;
        }
    }

    private static (string? FirstName, string? LastName) SplitName(string? fullName)
    {
        if (string.IsNullOrWhiteSpace(fullName))
        {
            return (null, null);
        }

        var parts = fullName.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length == 1)
        {
            return (parts[0], null);
        }

        return (parts[0], string.Join(' ', parts.Skip(1)));
    }
}
