// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UserServiceImplTests.cs" company="Electronic-Paradise">
//   © Electronic-Paradise. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using UserService.Abstraction.DTOs;
using UserService.Abstraction.Models;
using UserService.Core.Business;
using UserService.Core.Repository;
using Xunit;

namespace UserService.Core.Test.Business
{
    public class UserServiceImplTests
    {
        private readonly Mock<IUserRepository> userRepository;
        private readonly Mock<ILogger<UserServiceImpl>> logger;

        public UserServiceImplTests()
        {
            this.userRepository = new Mock<IUserRepository>();
            this.logger = new Mock<ILogger<UserServiceImpl>>();
        }

        /// <summary>
        /// Verifies that the UserServiceImpl constructor initializes successfully when all dependencies are provided.
        /// </summary>
        [Fact]
        public void GivenCtor_WhenAllSpecified_ThenInitializes()
        {
            // act
            var actual = new UserServiceImpl(this.userRepository.Object, this.logger.Object);

            // assert
            Assert.NotNull(actual);
        }

        /// <summary>
        /// Verifies that the UserServiceImpl constructor throws ArgumentNullException when userRepository is null.
        /// </summary>
        [Fact]
        public void GivenCtor_WhenUserRepositoryNull_ThenThrows()
        {
            // act & assert
            Assert.Throws<ArgumentNullException>(() => new UserServiceImpl(null!, this.logger.Object));
        }

        /// <summary>
        /// Verifies that the UserServiceImpl constructor throws ArgumentNullException when logger is null.
        /// </summary>
        [Fact]
        public void GivenCtor_WhenLoggerNull_ThenThrows()
        {
            // act & assert
            Assert.Throws<ArgumentNullException>(() => new UserServiceImpl(this.userRepository.Object, null!));
        }

        /// <summary>
        /// Verifies that GetByIdAsync returns the user profile when a valid ID is provided.
        /// </summary>
        [Fact]
        public async Task GivenValidId_WhenGetByIdAsync_ThenReturnsProfile()
        {
            // arrange
            var id = Guid.NewGuid();
            var profile = new UserProfile { Id = id, UserId = Guid.NewGuid(), FirstName = "Test" };

            this.userRepository.Setup(x => x.GetByIdAsync(id)).ReturnsAsync(profile);

            var service = new UserServiceImpl(this.userRepository.Object, this.logger.Object);

            // act
            var result = await service.GetByIdAsync(id);

            // assert
            Assert.NotNull(result);
            Assert.Equal(id, result.Id);
            this.userRepository.Verify(x => x.GetByIdAsync(id), Times.Once);
        }

        /// <summary>
        /// Verifies that GetByIdAsync returns null when the ID does not exist.
        /// </summary>
        [Fact]
        public async Task GivenNonExistentId_WhenGetByIdAsync_ThenReturnsNull()
        {
            // arrange
            var id = Guid.NewGuid();
            this.userRepository.Setup(x => x.GetByIdAsync(id)).ReturnsAsync((UserProfile?)null);

            var service = new UserServiceImpl(this.userRepository.Object, this.logger.Object);

            // act
            var result = await service.GetByIdAsync(id);

            // assert
            Assert.Null(result);
            this.userRepository.Verify(x => x.GetByIdAsync(id), Times.Once);
        }

        /// <summary>
        /// Verifies that GetByUserIdAsync returns the user profile when a valid user ID is provided.
        /// </summary>
        [Fact]
        public async Task GivenValidUserId_WhenGetByUserIdAsync_ThenReturnsProfile()
        {
            // arrange
            var userId = Guid.NewGuid();
            var profile = new UserProfile { Id = Guid.NewGuid(), UserId = userId, FirstName = "Test" };

            this.userRepository.Setup(x => x.GetByUserIdAsync(userId)).ReturnsAsync(profile);

            var service = new UserServiceImpl(this.userRepository.Object, this.logger.Object);

            // act
            var result = await service.GetByUserIdAsync(userId);

            // assert
            Assert.NotNull(result);
            Assert.Equal(userId, result.UserId);
            this.userRepository.Verify(x => x.GetByUserIdAsync(userId), Times.Once);
        }

        /// <summary>
        /// Verifies that PhoneNumberExistsAsync returns true when the phone number exists.
        /// </summary>
        [Fact]
        public async Task GivenExistingPhoneNumber_WhenPhoneNumberExistsAsync_ThenReturnsTrue()
        {
            // arrange
            var phoneNumber = "+1234567890";
            var profile = new UserProfile { Id = Guid.NewGuid(), PhoneNumber = phoneNumber };

            this.userRepository.Setup(x => x.GetByPhoneNumberAsync(phoneNumber)).ReturnsAsync(profile);

            var service = new UserServiceImpl(this.userRepository.Object, this.logger.Object);

            // act
            var result = await service.PhoneNumberExistsAsync(phoneNumber);

            // assert
            Assert.True(result);
            this.userRepository.Verify(x => x.GetByPhoneNumberAsync(phoneNumber), Times.Once);
        }

        /// <summary>
        /// Verifies that PhoneNumberExistsAsync returns false when the phone number does not exist.
        /// </summary>
        [Fact]
        public async Task GivenNonExistentPhoneNumber_WhenPhoneNumberExistsAsync_ThenReturnsFalse()
        {
            // arrange
            var phoneNumber = "+1234567890";
            this.userRepository.Setup(x => x.GetByPhoneNumberAsync(phoneNumber)).ReturnsAsync((UserProfile?)null);

            var service = new UserServiceImpl(this.userRepository.Object, this.logger.Object);

            // act
            var result = await service.PhoneNumberExistsAsync(phoneNumber);

            // assert
            Assert.False(result);
            this.userRepository.Verify(x => x.GetByPhoneNumberAsync(phoneNumber), Times.Once);
        }

        /// <summary>
        /// Verifies that CreateAsync creates a new user profile when valid data is provided.
        /// </summary>
        [Fact]
        public async Task GivenValidData_WhenCreateAsync_ThenCreatesProfile()
        {
            // arrange
            var dto = new CreateUserDto { UserId = Guid.NewGuid(), FirstName = "Test", PhoneNumber = "+1234567890" };
            var created = new UserProfile { Id = Guid.NewGuid(), UserId = dto.UserId, FirstName = dto.FirstName };

            this.userRepository.Setup(x => x.GetByPhoneNumberAsync(dto.PhoneNumber!)).ReturnsAsync((UserProfile?)null);
            this.userRepository.Setup(x => x.GetByUserIdAsync(dto.UserId)).ReturnsAsync((UserProfile?)null);
            this.userRepository.Setup(x => x.CreateAsync(It.IsAny<UserProfile>())).ReturnsAsync(created);

            var service = new UserServiceImpl(this.userRepository.Object, this.logger.Object);

            // act
            var result = await service.CreateAsync(dto);

            // assert
            Assert.NotNull(result);
            Assert.Equal(dto.UserId, result.UserId);
            this.userRepository.Verify(x => x.CreateAsync(It.IsAny<UserProfile>()), Times.Once);
        }

        /// <summary>
        /// Verifies that CreateAsync throws ArgumentException when the user ID is empty.
        /// </summary>
        [Fact]
        public async Task GivenEmptyUserId_WhenCreateAsync_ThenThrows()
        {
            // arrange
            var dto = new CreateUserDto { UserId = Guid.Empty, PhoneNumber = "+1234567890" };
            var service = new UserServiceImpl(this.userRepository.Object, this.logger.Object);

            // act & assert
            await Assert.ThrowsAsync<ArgumentException>(() => service.CreateAsync(dto));
        }

        /// <summary>
        /// Verifies that CreateAsync throws ArgumentException when the phone number is empty.
        /// </summary>
        [Fact]
        public async Task GivenEmptyPhoneNumber_WhenCreateAsync_ThenThrows()
        {
            // arrange
            var dto = new CreateUserDto { UserId = Guid.NewGuid(), PhoneNumber = string.Empty };
            var service = new UserServiceImpl(this.userRepository.Object, this.logger.Object);

            // act & assert
            await Assert.ThrowsAsync<ArgumentException>(() => service.CreateAsync(dto));
        }

        /// <summary>
        /// Verifies that CreateAsync throws ArgumentException when the phone number already exists.
        /// </summary>
        [Fact]
        public async Task GivenDuplicatePhoneNumber_WhenCreateAsync_ThenThrows()
        {
            // arrange
            var dto = new CreateUserDto { UserId = Guid.NewGuid(), PhoneNumber = "+1234567890" };
            var existing = new UserProfile { Id = Guid.NewGuid(), PhoneNumber = dto.PhoneNumber };

            this.userRepository.Setup(x => x.GetByPhoneNumberAsync(dto.PhoneNumber!)).ReturnsAsync(existing);

            var service = new UserServiceImpl(this.userRepository.Object, this.logger.Object);

            // act & assert
            await Assert.ThrowsAsync<ArgumentException>(() => service.CreateAsync(dto));
        }

        /// <summary>
        /// Verifies that UpdateAsync updates the user profile when valid data is provided.
        /// </summary>
        [Fact]
        public async Task GivenValidData_WhenUpdateAsync_ThenUpdatesProfile()
        {
            // arrange
            var id = Guid.NewGuid();
            var dto = new CreateUserDto { UserId = Guid.NewGuid(), FirstName = "Updated", PhoneNumber = "+1234567890" };
            var existing = new UserProfile { Id = id, UserId = dto.UserId, FirstName = "Old" };
            var updated = new UserProfile { Id = id, UserId = dto.UserId, FirstName = dto.FirstName };

            this.userRepository.Setup(x => x.GetByIdAsync(id)).ReturnsAsync(existing);
            this.userRepository.Setup(x => x.GetByPhoneNumberAsync(dto.PhoneNumber!)).ReturnsAsync((UserProfile?)null);
            this.userRepository.Setup(x => x.UpdateAsync(It.IsAny<UserProfile>())).ReturnsAsync(updated);

            var service = new UserServiceImpl(this.userRepository.Object, this.logger.Object);

            // act
            var result = await service.UpdateAsync(id, dto);

            // assert
            Assert.NotNull(result);
            Assert.Equal(dto.FirstName, result.FirstName);
            this.userRepository.Verify(x => x.UpdateAsync(It.IsAny<UserProfile>()), Times.Once);
        }

        /// <summary>
        /// Verifies that UpdateAsync returns null when the ID does not exist.
        /// </summary>
        [Fact]
        public async Task GivenNonExistentId_WhenUpdateAsync_ThenReturnsNull()
        {
            // arrange
            var id = Guid.NewGuid();
            var dto = new CreateUserDto { UserId = Guid.NewGuid(), FirstName = "Updated" };

            this.userRepository.Setup(x => x.GetByIdAsync(id)).ReturnsAsync((UserProfile?)null);

            var service = new UserServiceImpl(this.userRepository.Object, this.logger.Object);

            // act
            var result = await service.UpdateAsync(id, dto);

            // assert
            Assert.Null(result);
            this.userRepository.Verify(x => x.UpdateAsync(It.IsAny<UserProfile>()), Times.Never);
        }

        /// <summary>
        /// Verifies that DebitWalletAsync returns the new balance after debiting the wallet.
        /// </summary>
        [Fact]
        public async Task GivenValidData_WhenDebitWalletAsync_ThenReturnsNewBalance()
        {
            // arrange
            var id = Guid.NewGuid();
            var amount = 100m;

            this.userRepository.Setup(x => x.DebitWalletAsync(id, amount)).ReturnsAsync(400m);

            var service = new UserServiceImpl(this.userRepository.Object, this.logger.Object);

            // act
            var result = await service.DebitWalletAsync(id, amount);

            // assert
            Assert.Equal(400m, result);
            this.userRepository.Verify(x => x.DebitWalletAsync(id, amount), Times.Once);
        }

        /// <summary>
        /// Verifies that CreditWalletAsync returns the new balance after crediting the wallet.
        /// </summary>
        [Fact]
        public async Task GivenValidData_WhenCreditWalletAsync_ThenReturnsNewBalance()
        {
            // arrange
            var id = Guid.NewGuid();
            var amount = 100m;

            this.userRepository.Setup(x => x.CreditWalletAsync(id, amount)).ReturnsAsync(600m);

            var service = new UserServiceImpl(this.userRepository.Object, this.logger.Object);

            // act
            var result = await service.CreditWalletAsync(id, amount);

            // assert
            Assert.Equal(600m, result);
            this.userRepository.Verify(x => x.CreditWalletAsync(id, amount), Times.Once);
        }
    }
}