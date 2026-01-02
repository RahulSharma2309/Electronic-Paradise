using Microsoft.EntityFrameworkCore;
using UserService.Abstraction.Models;
using UserService.Core.Data;

namespace UserService.Core.Repository;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _db;
    public UserRepository(AppDbContext db) => _db = db;

    public async Task<UserProfile?> GetByIdAsync(Guid id)
    {
        return await _db.Users.FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<UserProfile?> GetByUserIdAsync(Guid userId)
    {
        return await _db.Users.FirstOrDefaultAsync(u => u.UserId == userId);
    }

    public async Task<UserProfile?> GetByPhoneNumberAsync(string phoneNumber)
    {
        return await _db.Users.FirstOrDefaultAsync(u => u.PhoneNumber == phoneNumber);
    }

    public async Task<UserProfile> CreateAsync(UserProfile profile)
    {
        _db.Users.Add(profile);
        await _db.SaveChangesAsync();
        return profile;
    }

    public async Task<UserProfile> UpdateAsync(UserProfile profile)
    {
        profile.UpdatedAt = DateTime.UtcNow;
        _db.Users.Update(profile);
        await _db.SaveChangesAsync();
        return profile;
    }

    public async Task<decimal> DebitWalletAsync(Guid id, decimal amount)
    {
        if (amount <= 0) throw new ArgumentException("Amount must be > 0", nameof(amount));
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == id);
        if (user == null) throw new KeyNotFoundException("User not found");
        if (user.WalletBalance < amount) throw new InvalidOperationException("Insufficient wallet balance");
        user.WalletBalance -= amount;
        user.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();
        return user.WalletBalance;
    }

    public async Task<decimal> CreditWalletAsync(Guid id, decimal amount)
    {
        if (amount <= 0) throw new ArgumentException("Amount must be > 0", nameof(amount));
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == id);
        if (user == null) throw new KeyNotFoundException("User not found");
        user.WalletBalance += amount;
        user.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();
        return user.WalletBalance;
    }
    
    public async Task<List<UserProfile>> GetAllAsync()
    {
        return await _db.Users.ToListAsync();
    }
}




