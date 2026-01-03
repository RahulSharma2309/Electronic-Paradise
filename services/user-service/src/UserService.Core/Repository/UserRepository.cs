// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UserRepository.cs" company="Electronic-Paradise">
//   © Electronic-Paradise. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UserService.Abstraction.Models;
using UserService.Core.Data;

namespace UserService.Core.Repository;

/// <summary>
/// Implements user profile data access operations using Entity Framework Core.
/// </summary>
public class UserRepository : IUserRepository
{
    private readonly AppDbContext _db;
    private readonly ILogger<UserRepository> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserRepository"/> class.
    /// </summary>
    /// <param name="db">The database context.</param>
    /// <param name="logger">The logger instance for structured logging.</param>
    /// <exception cref="ArgumentNullException">Thrown when any parameter is null.</exception>
    public UserRepository(AppDbContext db, ILogger<UserRepository> logger)
    {
        _db = db ?? throw new ArgumentNullException(nameof(db));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc/>
    public async Task<UserProfile?> GetByIdAsync(Guid id)
    {
        try
        {
            _logger.LogDebug("Searching for user profile by ID: {ProfileId}", id);

            var profile = await _db.Users.FirstOrDefaultAsync(u => u.Id == id);

            if (profile == null)
            {
                _logger.LogDebug("User profile not found with ID: {ProfileId}", id);
            }
            else
            {
                _logger.LogDebug("User profile found: {ProfileId}, UserId: {UserId}", profile.Id, profile.UserId);
            }

            return profile;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching for user profile by ID: {ProfileId}", id);
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<UserProfile?> GetByUserIdAsync(Guid userId)
    {
        try
        {
            _logger.LogDebug("Searching for user profile by UserId: {UserId}", userId);

            var profile = await _db.Users.FirstOrDefaultAsync(u => u.UserId == userId);

            if (profile == null)
            {
                _logger.LogDebug("User profile not found with UserId: {UserId}", userId);
            }
            else
            {
                _logger.LogDebug("User profile found: {ProfileId}, UserId: {UserId}", profile.Id, profile.UserId);
            }

            return profile;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching for user profile by UserId: {UserId}", userId);
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<UserProfile?> GetByPhoneNumberAsync(string phoneNumber)
    {
        if (string.IsNullOrWhiteSpace(phoneNumber))
        {
            throw new ArgumentException("Phone number cannot be null or whitespace.", nameof(phoneNumber));
        }

        try
        {
            _logger.LogDebug("Searching for user profile by phone number: {PhoneNumber}", phoneNumber);

            var profile = await _db.Users.FirstOrDefaultAsync(u => u.PhoneNumber == phoneNumber);

            if (profile == null)
            {
                _logger.LogDebug("User profile not found with phone number: {PhoneNumber}", phoneNumber);
            }
            else
            {
                _logger.LogDebug("User profile found: {ProfileId}, Phone: {PhoneNumber}", profile.Id, phoneNumber);
            }

            return profile;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching for user profile by phone number: {PhoneNumber}", phoneNumber);
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<UserProfile> CreateAsync(UserProfile profile)
    {
        if (profile == null)
        {
            throw new ArgumentNullException(nameof(profile));
        }

        try
        {
            _logger.LogDebug("Creating user profile for UserId: {UserId}", profile.UserId);

            _db.Users.Add(profile);
            await _db.SaveChangesAsync();

            _logger.LogInformation("User profile created successfully: {ProfileId}, UserId: {UserId}", profile.Id, profile.UserId);
            return profile;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create user profile for UserId: {UserId}", profile.UserId);
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<UserProfile> UpdateAsync(UserProfile profile)
    {
        if (profile == null)
        {
            throw new ArgumentNullException(nameof(profile));
        }

        try
        {
            _logger.LogDebug("Updating user profile: {ProfileId}, UserId: {UserId}", profile.Id, profile.UserId);

            profile.UpdatedAt = DateTime.UtcNow;
            _db.Users.Update(profile);
            await _db.SaveChangesAsync();

            _logger.LogInformation("User profile updated successfully: {ProfileId}, UserId: {UserId}", profile.Id, profile.UserId);
            return profile;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to update user profile: {ProfileId}, UserId: {UserId}", profile.Id, profile.UserId);
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<decimal> DebitWalletAsync(Guid id, decimal amount)
    {
        if (amount <= 0)
        {
            _logger.LogWarning("Debit wallet failed: Invalid amount {Amount} for ProfileId {ProfileId}", amount, id);
            throw new ArgumentException("Amount must be > 0", nameof(amount));
        }

        try
        {
            _logger.LogDebug("Debiting wallet for ProfileId {ProfileId}, Amount: {Amount}", id, amount);

            var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
            {
                _logger.LogWarning("Debit wallet failed: User not found with ID {ProfileId}", id);
                throw new KeyNotFoundException("User not found");
            }

            if (user.WalletBalance < amount)
            {
                _logger.LogWarning("Debit wallet failed: Insufficient balance for ProfileId {ProfileId}. Balance: {Balance}, Requested: {Amount}", id, user.WalletBalance, amount);
                throw new InvalidOperationException("Insufficient wallet balance");
            }

            user.WalletBalance -= amount;
            user.UpdatedAt = DateTime.UtcNow;
            await _db.SaveChangesAsync();

            _logger.LogInformation("Wallet debited successfully for ProfileId {ProfileId}, New Balance: {NewBalance}", id, user.WalletBalance);
            return user.WalletBalance;
        }
        catch (KeyNotFoundException)
        {
            throw;
        }
        catch (InvalidOperationException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error debiting wallet for ProfileId {ProfileId}, Amount: {Amount}", id, amount);
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<decimal> CreditWalletAsync(Guid id, decimal amount)
    {
        if (amount <= 0)
        {
            _logger.LogWarning("Credit wallet failed: Invalid amount {Amount} for ProfileId {ProfileId}", amount, id);
            throw new ArgumentException("Amount must be > 0", nameof(amount));
        }

        try
        {
            _logger.LogDebug("Crediting wallet for ProfileId {ProfileId}, Amount: {Amount}", id, amount);

            var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
            {
                _logger.LogWarning("Credit wallet failed: User not found with ID {ProfileId}", id);
                throw new KeyNotFoundException("User not found");
            }

            user.WalletBalance += amount;
            user.UpdatedAt = DateTime.UtcNow;
            await _db.SaveChangesAsync();

            _logger.LogInformation("Wallet credited successfully for ProfileId {ProfileId}, New Balance: {NewBalance}", id, user.WalletBalance);
            return user.WalletBalance;
        }
        catch (KeyNotFoundException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error crediting wallet for ProfileId {ProfileId}, Amount: {Amount}", id, amount);
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<List<UserProfile>> GetAllAsync()
    {
        try
        {
            _logger.LogDebug("Retrieving all user profiles");

            var profiles = await _db.Users.ToListAsync();

            _logger.LogDebug("Retrieved {Count} user profiles", profiles.Count);
            return profiles;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all user profiles");
            throw;
        }
    }
}
