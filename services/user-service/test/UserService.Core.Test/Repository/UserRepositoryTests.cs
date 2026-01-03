// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UserRepositoryTests.cs" company="Electronic-Paradise">
//   © Electronic-Paradise. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using UserService.Abstraction.Models;
using UserService.Core.Data;
using UserService.Core.Repository;
using Xunit;

namespace UserService.Core.Test.Repository
{
    public class UserRepositoryTests
    {
        private readonly AppDbContext dbContext;
        private readonly Mock<ILogger<UserRepository>> logger;

        public UserRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            this.dbContext = new AppDbContext(options);
            this.logger = new Mock<ILogger<UserRepository>>();
        }

        /// <summary>
        /// Verifies that the UserRepository constructor initializes successfully when all dependencies are provided.
        /// </summary>
        [Fact]
        public void GivenCtor_WhenAllSpecified_ThenInitializes()
        {
            // act
            var actual = new UserRepository(this.dbContext, this.logger.Object);

            // assert
            Assert.NotNull(actual);
        }

        /// <summary>
        /// Verifies that the UserRepository constructor throws ArgumentNullException when dbContext is null.
        /// </summary>
        [Fact]
        public void GivenCtor_WhenDbContextNull_ThenThrows()
        {
            // act & assert
            Assert.Throws<ArgumentNullException>(() => new UserRepository(null!, this.logger.Object));
        }

        /// <summary>
        /// Verifies that the UserRepository constructor throws ArgumentNullException when logger is null.
        /// </summary>
        [Fact]
        public void GivenCtor_WhenLoggerNull_ThenThrows()
        {
            // act & assert
            Assert.Throws<ArgumentNullException>(() => new UserRepository(this.dbContext, null!));
        }

        /// <summary>
        /// Verifies that CreateAsync successfully creates a user profile in the database.
        /// </summary>
        [Fact]
        public async Task GivenValidProfile_WhenCreateAsync_ThenCreatesProfile()
        {
            // arrange
            var profile = new UserProfile { UserId = Guid.NewGuid(), FirstName = "Test", PhoneNumber = "+1234567890" };
            var repository = new UserRepository(this.dbContext, this.logger.Object);

            // act
            var result = await repository.CreateAsync(profile);

            // assert
            Assert.NotNull(result);
            Assert.Equal(profile.FirstName, result.FirstName);

            var saved = await this.dbContext.Users.FindAsync(result.Id);
            Assert.NotNull(saved);
        }

        /// <summary>
        /// Verifies that CreateAsync throws ArgumentNullException when the profile is null.
        /// </summary>
        [Fact]
        public async Task GivenNullProfile_WhenCreateAsync_ThenThrows()
        {
            // arrange
            var repository = new UserRepository(this.dbContext, this.logger.Object);

            // act & assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => repository.CreateAsync(null!));
        }

        /// <summary>
        /// Verifies that GetByIdAsync returns the user profile when an existing ID is provided.
        /// </summary>
        [Fact]
        public async Task GivenExistingId_WhenGetByIdAsync_ThenReturnsProfile()
        {
            // arrange
            var profile = new UserProfile { UserId = Guid.NewGuid(), FirstName = "Test", PhoneNumber = "+1234567890" };
            this.dbContext.Users.Add(profile);
            await this.dbContext.SaveChangesAsync();

            var repository = new UserRepository(this.dbContext, this.logger.Object);

            // act
            var result = await repository.GetByIdAsync(profile.Id);

            // assert
            Assert.NotNull(result);
            Assert.Equal(profile.Id, result.Id);
        }

        /// <summary>
        /// Verifies that GetByIdAsync returns null when the ID does not exist.
        /// </summary>
        [Fact]
        public async Task GivenNonExistentId_WhenGetByIdAsync_ThenReturnsNull()
        {
            // arrange
            var repository = new UserRepository(this.dbContext, this.logger.Object);

            // act
            var result = await repository.GetByIdAsync(Guid.NewGuid());

            // assert
            Assert.Null(result);
        }

        /// <summary>
        /// Verifies that GetByUserIdAsync returns the user profile when an existing user ID is provided.
        /// </summary>
        [Fact]
        public async Task GivenExistingUserId_WhenGetByUserIdAsync_ThenReturnsProfile()
        {
            // arrange
            var userId = Guid.NewGuid();
            var profile = new UserProfile { UserId = userId, FirstName = "Test", PhoneNumber = "+1234567890" };
            this.dbContext.Users.Add(profile);
            await this.dbContext.SaveChangesAsync();

            var repository = new UserRepository(this.dbContext, this.logger.Object);

            // act
            var result = await repository.GetByUserIdAsync(userId);

            // assert
            Assert.NotNull(result);
            Assert.Equal(userId, result.UserId);
        }

        /// <summary>
        /// Verifies that GetByPhoneNumberAsync returns the user profile when an existing phone number is provided.
        /// </summary>
        [Fact]
        public async Task GivenExistingPhoneNumber_WhenGetByPhoneNumberAsync_ThenReturnsProfile()
        {
            // arrange
            var phoneNumber = "+1234567890";
            var profile = new UserProfile { UserId = Guid.NewGuid(), FirstName = "Test", PhoneNumber = phoneNumber };
            this.dbContext.Users.Add(profile);
            await this.dbContext.SaveChangesAsync();

            var repository = new UserRepository(this.dbContext, this.logger.Object);

            // act
            var result = await repository.GetByPhoneNumberAsync(phoneNumber);

            // assert
            Assert.NotNull(result);
            Assert.Equal(phoneNumber, result.PhoneNumber);
        }

        /// <summary>
        /// Verifies that GetByPhoneNumberAsync throws ArgumentException when phone number is null.
        /// </summary>
        [Fact]
        public async Task GivenNullPhoneNumber_WhenGetByPhoneNumberAsync_ThenThrows()
        {
            // arrange
            var repository = new UserRepository(this.dbContext, this.logger.Object);

            // act & assert
            await Assert.ThrowsAsync<ArgumentException>(() => repository.GetByPhoneNumberAsync(null!));
        }

        /// <summary>
        /// Verifies that UpdateAsync successfully updates the user profile in the database.
        /// </summary>
        [Fact]
        public async Task GivenValidProfile_WhenUpdateAsync_ThenUpdatesProfile()
        {
            // arrange
            var profile = new UserProfile { UserId = Guid.NewGuid(), FirstName = "Old", PhoneNumber = "+1234567890" };
            this.dbContext.Users.Add(profile);
            await this.dbContext.SaveChangesAsync();

            profile.FirstName = "Updated";
            var repository = new UserRepository(this.dbContext, this.logger.Object);

            // act
            var result = await repository.UpdateAsync(profile);

            // assert
            Assert.Equal("Updated", result.FirstName);

            var updated = await this.dbContext.Users.FindAsync(profile.Id);
            Assert.Equal("Updated", updated!.FirstName);
        }

        /// <summary>
        /// Verifies that UpdateAsync throws ArgumentNullException when the profile is null.
        /// </summary>
        [Fact]
        public async Task GivenNullProfile_WhenUpdateAsync_ThenThrows()
        {
            // arrange
            var repository = new UserRepository(this.dbContext, this.logger.Object);

            // act & assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => repository.UpdateAsync(null!));
        }

        /// <summary>
        /// Verifies that DebitWalletAsync successfully debits the wallet and returns the new balance.
        /// </summary>
        [Fact]
        public async Task GivenValidData_WhenDebitWalletAsync_ThenDebitsWallet()
        {
            // arrange
            var profile = new UserProfile
            {
                UserId = Guid.NewGuid(),
                FirstName = "Test",
                PhoneNumber = "+1234567890",
                WalletBalance = 500m,
            };
            this.dbContext.Users.Add(profile);
            await this.dbContext.SaveChangesAsync();

            var repository = new UserRepository(this.dbContext, this.logger.Object);

            // act
            var result = await repository.DebitWalletAsync(profile.Id, 100m);

            // assert
            Assert.Equal(400m, result);

            var updated = await this.dbContext.Users.FindAsync(profile.Id);
            Assert.Equal(400m, updated!.WalletBalance);
        }

        /// <summary>
        /// Verifies that DebitWalletAsync throws InvalidOperationException when there is insufficient balance.
        /// </summary>
        [Fact]
        public async Task GivenInsufficientBalance_WhenDebitWalletAsync_ThenThrows()
        {
            // arrange
            var profile = new UserProfile
            {
                UserId = Guid.NewGuid(),
                FirstName = "Test",
                PhoneNumber = "+1234567890",
                WalletBalance = 50m,
            };
            this.dbContext.Users.Add(profile);
            await this.dbContext.SaveChangesAsync();

            var repository = new UserRepository(this.dbContext, this.logger.Object);

            // act & assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => repository.DebitWalletAsync(profile.Id, 100m));
        }

        /// <summary>
        /// Verifies that DebitWalletAsync throws KeyNotFoundException when the user does not exist.
        /// </summary>
        [Fact]
        public async Task GivenNonExistentUser_WhenDebitWalletAsync_ThenThrows()
        {
            // arrange
            var repository = new UserRepository(this.dbContext, this.logger.Object);

            // act & assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => repository.DebitWalletAsync(Guid.NewGuid(), 100m));
        }

        /// <summary>
        /// Verifies that CreditWalletAsync successfully credits the wallet and returns the new balance.
        /// </summary>
        [Fact]
        public async Task GivenValidData_WhenCreditWalletAsync_ThenCreditsWallet()
        {
            // arrange
            var profile = new UserProfile
            {
                UserId = Guid.NewGuid(),
                FirstName = "Test",
                PhoneNumber = "+1234567890",
                WalletBalance = 500m,
            };
            this.dbContext.Users.Add(profile);
            await this.dbContext.SaveChangesAsync();

            var repository = new UserRepository(this.dbContext, this.logger.Object);

            // act
            var result = await repository.CreditWalletAsync(profile.Id, 100m);

            // assert
            Assert.Equal(600m, result);

            var updated = await this.dbContext.Users.FindAsync(profile.Id);
            Assert.Equal(600m, updated!.WalletBalance);
        }

        /// <summary>
        /// Verifies that CreditWalletAsync throws KeyNotFoundException when the user does not exist.
        /// </summary>
        [Fact]
        public async Task GivenNonExistentUser_WhenCreditWalletAsync_ThenThrows()
        {
            // arrange
            var repository = new UserRepository(this.dbContext, this.logger.Object);

            // act & assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => repository.CreditWalletAsync(Guid.NewGuid(), 100m));
        }

        /// <summary>
        /// Verifies that DebitWalletAsync throws ArgumentException when amount is negative.
        /// </summary>
        [Fact]
        public async Task GivenInvalidAmount_WhenDebitWalletAsync_ThenThrows()
        {
            // arrange
            var repository = new UserRepository(this.dbContext, this.logger.Object);

            // act & assert
            await Assert.ThrowsAsync<ArgumentException>(() => repository.DebitWalletAsync(Guid.NewGuid(), -10m));
        }

        /// <summary>
        /// Verifies that CreditWalletAsync throws ArgumentException when amount is zero or negative.
        /// </summary>
        [Fact]
        public async Task GivenInvalidAmount_WhenCreditWalletAsync_ThenThrows()
        {
            // arrange
            var repository = new UserRepository(this.dbContext, this.logger.Object);

            // act & assert
            await Assert.ThrowsAsync<ArgumentException>(() => repository.CreditWalletAsync(Guid.NewGuid(), 0m));
        }

        /// <summary>
        /// Verifies that GetAllAsync returns all user profiles from the database.
        /// </summary>
        [Fact]
        public async Task WhenGetAllAsync_ThenReturnsAllProfiles()
        {
            // arrange
            this.dbContext.Users.Add(new UserProfile { UserId = Guid.NewGuid(), PhoneNumber = "+1111111111" });
            this.dbContext.Users.Add(new UserProfile { UserId = Guid.NewGuid(), PhoneNumber = "+2222222222" });
            await this.dbContext.SaveChangesAsync();

            var repository = new UserRepository(this.dbContext, this.logger.Object);

            // act
            var result = await repository.GetAllAsync();

            // assert
            Assert.Equal(2, result.Count);
        }
    }
}