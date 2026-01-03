using UserService.Abstraction.DTOs;

namespace UserService.Core.Business;

public interface IUserService
{
    Task<UserProfileDto?> GetByIdAsync(Guid id);
    Task<UserProfileDto?> GetByUserIdAsync(Guid userId);
    Task<bool> PhoneNumberExistsAsync(string phoneNumber);
    Task<UserProfileDto> CreateAsync(CreateUserDto dto);
    Task<UserProfileDto?> UpdateAsync(Guid id, CreateUserDto dto);
    Task<decimal> DebitWalletAsync(Guid id, decimal amount);
    Task<decimal> CreditWalletAsync(Guid id, decimal amount);
}





