// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UserServiceImpl.cs" company="FreshHarvest-Market">
//   © FreshHarvest-Market. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using Microsoft.Extensions.Logging;
using UserService.Abstraction.DTOs.Requests;
using UserService.Abstraction.DTOs.Responses;
using UserService.Abstraction.Models;
using UserService.Core.Mappers;
using UserService.Core.Repository;

namespace UserService.Core.Business;

/// <summary>
/// Implements user profile business logic operations.
/// </summary>
public class UserServiceImpl : IUserService
{
    private readonly IUserRepository _repo;
    private readonly IUserProfileMapper _mapper;
    private readonly ILogger<UserServiceImpl> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserServiceImpl"/> class.
    /// </summary>
    /// <param name="repo">The user repository for data access.</param>
    /// <param name="mapper">The user profile mapper.</param>
    /// <param name="logger">The logger instance for structured logging.</param>
    /// <exception cref="ArgumentNullException">Thrown when any parameter is null.</exception>
    public UserServiceImpl(IUserRepository repo, IUserProfileMapper mapper, ILogger<UserServiceImpl> logger)
    {
        _repo = repo ?? throw new ArgumentNullException(nameof(repo));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc/>
    public async Task<UserProfileDetailResponse?> GetByIdAsync(Guid id)
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
            return _mapper.ToDetailResponse(m);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving user profile by ID: {ProfileId}", id);
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<UserProfileResponse?> GetByUserIdAsync(Guid userId)
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
            return _mapper.ToResponse(m);
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
    public async Task<UserProfileDetailResponse> CreateAsync(CreateUserProfileRequest request)
    {
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        try
        {
            _logger.LogInformation("Creating user profile for UserId: {UserId}", request.UserId);

            // Check for duplicate phone number
            if (!string.IsNullOrWhiteSpace(request.PhoneNumber))
            {
                var existingPhone = await _repo.GetByPhoneNumberAsync(request.PhoneNumber);
                if (existingPhone != null)
                {
                    _logger.LogWarning("Create user profile failed: Phone number already registered for UserId {UserId}", request.UserId);
                    throw new ArgumentException("Phone number already registered");
                }
            }

            // If a profile already exists for this UserId, update it instead
            var existing = await _repo.GetByUserIdAsync(request.UserId);
            if (existing != null)
            {
                _logger.LogInformation("User profile already exists for UserId {UserId}, updating instead", request.UserId);

                existing.FirstName = request.FirstName;
                existing.LastName = request.LastName;
                existing.Address = request.Address;
                existing.PhoneNumber = request.PhoneNumber;
                existing.UpdatedAt = DateTime.UtcNow;

                var updated = await _repo.UpdateAsync(existing);

                _logger.LogInformation("User profile updated successfully: {ProfileId}, UserId: {UserId}", updated.Id, updated.UserId);
                return _mapper.ToDetailResponse(updated);
            }

            var model = _mapper.ToEntity(request);
            var created = await _repo.CreateAsync(model);

            _logger.LogInformation("User profile created successfully: {ProfileId}, UserId: {UserId}", created.Id, created.UserId);
            return _mapper.ToDetailResponse(created);
        }
        catch (ArgumentException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create user profile for UserId {UserId}", request.UserId);
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<UserProfileDetailResponse?> UpdateAsync(Guid id, UpdateUserProfileRequest request)
    {
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request));
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

            // Check for duplicate phone number if updating phone
            if (!string.IsNullOrWhiteSpace(request.PhoneNumber))
            {
                var existingPhone = await _repo.GetByPhoneNumberAsync(request.PhoneNumber);
                if (existingPhone != null && existingPhone.Id != id)
                {
                    _logger.LogWarning("Update failed: Phone number already registered for ProfileId {ProfileId}", id);
                    throw new ArgumentException("Phone number already registered");
                }
            }

            _mapper.UpdateEntity(existing, request);
            var updated = await _repo.UpdateAsync(existing);

            _logger.LogInformation("User profile updated successfully: {ProfileId}, UserId: {UserId}", updated.Id, updated.UserId);
            return _mapper.ToDetailResponse(updated);
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
    public async Task<WalletBalanceResponse> AddBalanceAsync(Guid userId, AddBalanceRequest request)
    {
        try
        {
            _logger.LogInformation("Adding balance for UserId {UserId}, Amount: {Amount}", userId, request.Amount);

            var profile = await _repo.GetByUserIdAsync(userId);
            if (profile == null)
            {
                throw new KeyNotFoundException($"User profile not found for UserId {userId}");
            }

            profile.WalletBalance += request.Amount;
            profile.UpdatedAt = DateTime.UtcNow;
            await _repo.UpdateAsync(profile);

            _logger.LogInformation("Balance added successfully for UserId {UserId}, New Balance: {NewBalance}", userId, profile.WalletBalance);
            return _mapper.ToWalletBalanceResponse(userId, profile.WalletBalance, $"Successfully added ${request.Amount:F2}. New balance: ${profile.WalletBalance:F2}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to add balance for UserId {UserId}, Amount: {Amount}", userId, request.Amount);
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<WalletBalanceResponse> DebitWalletAsync(Guid userId, decimal amount)
    {
        try
        {
            _logger.LogInformation("Debiting wallet for UserId {UserId}, Amount: {Amount}", userId, amount);

            var profile = await _repo.GetByUserIdAsync(userId);
            if (profile == null)
            {
                throw new KeyNotFoundException($"User profile not found for UserId {userId}");
            }

            var newBalance = await _repo.DebitWalletAsync(profile.Id, amount);

            _logger.LogInformation("Wallet debited successfully for UserId {UserId}, New Balance: {NewBalance}", userId, newBalance);
            return _mapper.ToWalletBalanceResponse(userId, newBalance, $"Debited ${amount:F2}. New balance: ${newBalance:F2}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to debit wallet for UserId {UserId}, Amount: {Amount}", userId, amount);
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<WalletBalanceResponse> CreditWalletAsync(Guid userId, decimal amount)
    {
        try
        {
            _logger.LogInformation("Crediting wallet for UserId {UserId}, Amount: {Amount}", userId, amount);

            var profile = await _repo.GetByUserIdAsync(userId);
            if (profile == null)
            {
                throw new KeyNotFoundException($"User profile not found for UserId {userId}");
            }

            var newBalance = await _repo.CreditWalletAsync(profile.Id, amount);

            _logger.LogInformation("Wallet credited successfully for UserId {UserId}, New Balance: {NewBalance}", userId, newBalance);
            return _mapper.ToWalletBalanceResponse(userId, newBalance, $"Credited ${amount:F2}. New balance: ${newBalance:F2}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to credit wallet for UserId {UserId}, Amount: {Amount}", userId, amount);
            throw;
        }
    }
}
