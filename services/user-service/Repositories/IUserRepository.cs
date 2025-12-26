using UserService.Models;

namespace UserService.Repositories
{
    public interface IUserRepository
    {
        Task<UserProfile?> GetByIdAsync(Guid id);
        Task<UserProfile?> GetByUserIdAsync(Guid userId);
        Task<UserProfile?> GetByPhoneNumberAsync(string phoneNumber);
        Task<UserProfile> CreateAsync(UserProfile profile);
        Task<UserProfile> UpdateAsync(UserProfile profile);
        Task<decimal> DebitWalletAsync(Guid id, decimal amount);
        Task<decimal> CreditWalletAsync(Guid id, decimal amount);
        Task<List<UserProfile>> GetAllAsync();
    }
}
