using UserService.Dtos;
using UserService.Models;
using UserService.Repositories;

namespace UserService.Services
{
    public class UserServiceImpl : IUserService
    {
        private readonly IUserRepository _repo;
        private readonly ILogger<UserServiceImpl> _logger;
        public UserServiceImpl(IUserRepository repo, ILogger<UserServiceImpl> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        public async Task<UserProfileDto?> GetByIdAsync(Guid id)
        {
            var m = await _repo.GetByIdAsync(id);
            return m is null ? null : UserProfileDto.FromModel(m);
        }

        public async Task<UserProfileDto?> GetByUserIdAsync(Guid userId)
        {
            var m = await _repo.GetByUserIdAsync(userId);
            return m is null ? null : UserProfileDto.FromModel(m);
        }

        public async Task<bool> PhoneNumberExistsAsync(string phoneNumber)
        {
            var profile = await _repo.GetByPhoneNumberAsync(phoneNumber);
            return profile != null;
        }

        public async Task<UserProfileDto> CreateAsync(CreateUserDto dto)
        {
            // Backend validation
            if (dto.UserId == Guid.Empty)
                throw new ArgumentException("UserId is required");
            if (string.IsNullOrWhiteSpace(dto.PhoneNumber))
                throw new ArgumentException("Phone number is required");
            if (!System.Text.RegularExpressions.Regex.IsMatch(dto.PhoneNumber, @"^\+?\d{10,15}$"))
                throw new ArgumentException("Invalid phone number format");
            // Check for duplicate phone number
            var existingPhone = await _repo.GetByPhoneNumberAsync(dto.PhoneNumber);
            if (existingPhone != null)
                throw new ArgumentException("Phone number already registered");

            try
            {
                // If a profile already exists for this UserId, update it instead
                var existing = await _repo.GetByUserIdAsync(dto.UserId);
                if (existing != null)
                {
                    existing.FirstName = dto.FirstName;
                    existing.LastName = dto.LastName;
                    existing.Address = dto.Address;
                    existing.PhoneNumber = dto.PhoneNumber;
                    var updated = await _repo.UpdateAsync(existing);
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
                    UpdatedAt = DateTime.UtcNow
                };

                var created = await _repo.CreateAsync(model);
                return UserProfileDto.FromModel(created);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create user profile for UserId {UserId}", dto.UserId);
                throw;
            }
        }

        public async Task<UserProfileDto?> UpdateAsync(Guid id, CreateUserDto dto)
        {
            var existing = await _repo.GetByIdAsync(id);
            if (existing == null) return null;

            // Backend validation
            if (!string.IsNullOrWhiteSpace(dto.PhoneNumber))
            {
                if (!System.Text.RegularExpressions.Regex.IsMatch(dto.PhoneNumber, @"^\+?\d{10,15}$"))
                    throw new ArgumentException("Invalid phone number format");
                // Check for duplicate phone number (excluding current user)
                var existingPhone = await _repo.GetByPhoneNumberAsync(dto.PhoneNumber);
                if (existingPhone != null && existingPhone.Id != id)
                    throw new ArgumentException("Phone number already registered");
            }

            try
            {
                // don't change UserId on update
                existing.FirstName = dto.FirstName;
                existing.LastName = dto.LastName;
                existing.Address = dto.Address;
                existing.PhoneNumber = dto.PhoneNumber;

                var updated = await _repo.UpdateAsync(existing);
                return UserProfileDto.FromModel(updated);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update user profile for Id {Id}", id);
                throw;
            }
        }

        public async Task<decimal> DebitWalletAsync(Guid id, decimal amount)
        {
            return await _repo.DebitWalletAsync(id, amount);
        }

        public async Task<decimal> CreditWalletAsync(Guid id, decimal amount)
        {
            return await _repo.CreditWalletAsync(id, amount);
        }
    }
}
