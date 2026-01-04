// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UserServiceImpl.cs" company="Electronic-Paradise">
//   © Electronic-Paradise. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using Microsoft.Extensions.Logging;
using UserService.Abstraction.DTOs;
using UserService.Abstraction.Models;
using UserService.Core.Repository;

namespace UserService.Core.Business;

/// <summary>
/// Implements user profile business logic operations.
/// </summary>
public class UserServiceImpl : IUserService
{
    private readonly IUserRepository _repo;
    private readonly ILogger<UserServiceImpl> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserServiceImpl"/> class.
    /// </summary>
    /// <param name="repo">The user repository for data access.</param>
    /// <param name="logger">The logger instance for structured logging.</param>
    /// <exception cref="ArgumentNullException">Thrown when any parameter is null.</exception>
    public UserServiceImpl(IUserRepository repo, ILogger<UserServiceImpl> logger)
    {
        _repo = repo ?? throw new ArgumentNullException(nameof(repo));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc/>
    public async Task<UserProfileDto?> GetByIdAsync(Guid id)
    {
        try
        {
            _logger.LogDebug("Retrieving user profile by ID: {ProfileId}", id);

            var m = await _repo.GetByIdAsync(id);

            if (m == null)
            {
                _logger.LogWarning("User profile not found with ID: {ProfileId}", id);
                return null;
            }

            _logger.LogDebug("User profile retrieved successfully: {ProfileId}, UserId: {UserId}", m.Id, m.UserId);
            return UserProfileDto.FromModel(m);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving user profile by ID: {ProfileId}", id);
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<UserProfileDto?> GetByUserIdAsync(Guid userId)
    {
        try
        {
            _logger.LogDebug("Retrieving user profile by UserId: {UserId}", userId);

            var m = await _repo.GetByUserIdAsync(userId);

            if (m == null)
            {
                _logger.LogWarning("User profile not found with UserId: {UserId}", userId);
                return null;
            }

            _logger.LogDebug("User profile retrieved successfully: {ProfileId}, UserId: {UserId}", m.Id, m.UserId);
            return UserProfileDto.FromModel(m);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving user profile by UserId: {UserId}", userId);
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<bool> PhoneNumberExistsAsync(string phoneNumber)
    {
        if (string.IsNullOrWhiteSpace(phoneNumber))
        {
            throw new ArgumentException("Phone number cannot be null or whitespace.", nameof(phoneNumber));
        }

        try
        {
            _logger.LogDebug("Checking if phone number exists: {PhoneNumber}", phoneNumber);

            var profile = await _repo.GetByPhoneNumberAsync(phoneNumber);
            var exists = profile != null;

            _logger.LogDebug("Phone number exists check result for {PhoneNumber}: {Exists}", phoneNumber, exists);
            return exists;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking phone number existence: {PhoneNumber}", phoneNumber);
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<UserProfileDto> CreateAsync(CreateUserDto dto)
    {
        if (dto == null)
        {
            throw new ArgumentNullException(nameof(dto));
        }

        // Backend validation
        if (dto.UserId == Guid.Empty)
        {
            _logger.LogWarning("Create user profile failed: UserId is empty");
            throw new ArgumentException("UserId is required");
        }

        if (string.IsNullOrWhiteSpace(dto.PhoneNumber))
        {
            _logger.LogWarning("Create user profile failed: Phone number is empty for UserId {UserId}", dto.UserId);
            throw new ArgumentException("Phone number is required");
        }

        if (!System.Text.RegularExpressions.Regex.IsMatch(dto.PhoneNumber, @"^\+?\d{10,15}$"))
        {
            _logger.LogWarning("Create user profile failed: Invalid phone number format for UserId {UserId}", dto.UserId);
            throw new ArgumentException("Invalid phone number format");
        }

        try
        {
            _logger.LogInformation("Creating user profile for UserId: {UserId}", dto.UserId);

            // Check for duplicate phone number
            var existingPhone = await _repo.GetByPhoneNumberAsync(dto.PhoneNumber);
            if (existingPhone != null)
            {
                _logger.LogWarning("Create user profile failed: Phone number already registered for UserId {UserId}", dto.UserId);
                throw new ArgumentException("Phone number already registered");
            }

            // If a profile already exists for this UserId, update it instead
            var existing = await _repo.GetByUserIdAsync(dto.UserId);
            if (existing != null)
            {
                _logger.LogInformation("User profile already exists for UserId {UserId}, updating instead", dto.UserId);

                existing.FirstName = dto.FirstName;
                existing.LastName = dto.LastName;
                existing.Address = dto.Address;
                existing.PhoneNumber = dto.PhoneNumber;

                var updated = await _repo.UpdateAsync(existing);

                _logger.LogInformation("User profile updated successfully: {ProfileId}, UserId: {UserId}", updated.Id, updated.UserId);
                return UserProfileDto.FromModel(updated);
            }

            var model = new UserProfile
            {
                UserId = dto.UserId,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Address = dto.Address,
                PhoneNumber = dto.PhoneNumber,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };

            var created = await _repo.CreateAsync(model);

            _logger.LogInformation("User profile created successfully: {ProfileId}, UserId: {UserId}", created.Id, created.UserId);
            return UserProfileDto.FromModel(created);
        }
        catch (ArgumentException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create user profile for UserId {UserId}", dto.UserId);
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<UserProfileDto?> UpdateAsync(Guid id, CreateUserDto dto)
    {
        if (dto == null)
        {
            throw new ArgumentNullException(nameof(dto));
        }

        try
        {
            _logger.LogInformation("Updating user profile: {ProfileId}", id);

            var existing = await _repo.GetByIdAsync(id);
            if (existing == null)
            {
                _logger.LogWarning("Update failed: User profile not found with ID {ProfileId}", id);
                return null;
            }

            // Backend validation
            if (!string.IsNullOrWhiteSpace(dto.PhoneNumber))
            {
                if (!System.Text.RegularExpressions.Regex.IsMatch(dto.PhoneNumber, @"^\+?\d{10,15}$"))
                {
                    _logger.LogWarning("Update failed: Invalid phone number format for ProfileId {ProfileId}", id);
                    throw new ArgumentException("Invalid phone number format");
                }

                // Check for duplicate phone number (excluding current user)
                var existingPhone = await _repo.GetByPhoneNumberAsync(dto.PhoneNumber);
                if (existingPhone != null && existingPhone.Id != id)
                {
                    _logger.LogWarning("Update failed: Phone number already registered for ProfileId {ProfileId}", id);
                    throw new ArgumentException("Phone number already registered");
                }
            }

            // Don't change UserId on update
            existing.FirstName = dto.FirstName;
            existing.LastName = dto.LastName;
            existing.Address = dto.Address;
            existing.PhoneNumber = dto.PhoneNumber;

            var updated = await _repo.UpdateAsync(existing);

            _logger.LogInformation("User profile updated successfully: {ProfileId}, UserId: {UserId}", updated.Id, updated.UserId);
            return UserProfileDto.FromModel(updated);
        }
        catch (ArgumentException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to update user profile for Id {Id}", id);
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<decimal> DebitWalletAsync(Guid id, decimal amount)
    {
        try
        {
            _logger.LogInformation("Debiting wallet for ProfileId {ProfileId}, Amount: {Amount}", id, amount);

            var newBalance = await _repo.DebitWalletAsync(id, amount);

            _logger.LogInformation("Wallet debited successfully for ProfileId {ProfileId}, New Balance: {NewBalance}", id, newBalance);
            return newBalance;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to debit wallet for ProfileId {ProfileId}, Amount: {Amount}", id, amount);
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<decimal> CreditWalletAsync(Guid id, decimal amount)
    {
        try
        {
            _logger.LogInformation("Crediting wallet for ProfileId {ProfileId}, Amount: {Amount}", id, amount);

            var newBalance = await _repo.CreditWalletAsync(id, amount);

            _logger.LogInformation("Wallet credited successfully for ProfileId {ProfileId}, New Balance: {NewBalance}", id, newBalance);
            return newBalance;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to credit wallet for ProfileId {ProfileId}, Amount: {Amount}", id, amount);
            throw;
        }
    }
}
