using AuthService.Abstraction.Models;

namespace AuthService.Core.Repository;

public interface IUserRepository
{
    Task AddAsync(User user);
    Task<User?> FindByEmailAsync(string email);
    Task<User?> FindByIdAsync(Guid id);
    Task<bool> EmailExistsAsync(string email);
    Task UpdateAsync(User user);
    Task SaveChangesAsync();
}




