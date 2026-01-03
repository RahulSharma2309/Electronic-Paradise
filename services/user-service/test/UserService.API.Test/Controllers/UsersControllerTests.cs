// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UsersControllerTests.cs" company="Electronic-Paradise">
//   © Electronic-Paradise. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using UserService.Abstraction.DTOs;
using UserService.API.Controllers;
using UserService.Core.Business;
using Xunit;

namespace UserService.API.Test.Controllers
{
    public class UsersControllerTests
    {
        private readonly Mock<IUserService> userService;
        private readonly Mock<ILogger<UsersController>> logger;

        public UsersControllerTests()
        {
            this.userService = new Mock<IUserService>();
            this.logger = new Mock<ILogger<UsersController>>();
        }

        /// <summary>
        /// Verifies that the UsersController constructor initializes successfully when all dependencies are provided.
        /// </summary>
        [Fact]
        public void GivenCtor_WhenAllSpecified_ThenInitializes()
        {
            // act
            var actual = new UsersController(this.userService.Object, this.logger.Object);

            // assert
            Assert.NotNull(actual);
        }

        /// <summary>
        /// Verifies that the UsersController constructor throws ArgumentNullException when userService is null.
        /// </summary>
        [Fact]
        public void GivenCtor_WhenUserServiceNull_ThenThrows()
        {
            // act & assert
            Assert.Throws<ArgumentNullException>(() => new UsersController(null!, this.logger.Object));
        }

        /// <summary>
        /// Verifies that the UsersController constructor throws ArgumentNullException when logger is null.
        /// </summary>
        [Fact]
        public void GivenCtor_WhenLoggerNull_ThenThrows()
        {
            // act & assert
            Assert.Throws<ArgumentNullException>(() => new UsersController(this.userService.Object, null!));
        }

        /// <summary>
        /// Verifies that GetById returns OK (200) with user profile when a valid ID is provided.
        /// </summary>
        [Fact]
        public async Task GivenValidId_WhenGetById_ThenReturnsOk()
        {
            // arrange
            var id = Guid.NewGuid();
            var profile = new UserProfileDto { Id = id, UserId = Guid.NewGuid(), FirstName = "Test" };

            this.userService.Setup(x => x.GetByIdAsync(id)).ReturnsAsync(profile);

            var controller = new UsersController(this.userService.Object, this.logger.Object);

            // act
            var result = await controller.GetById(id);

            // assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.OK, okResult.StatusCode);
            this.userService.Verify(x => x.GetByIdAsync(id), Times.Once);
        }

        /// <summary>
        /// Verifies that GetById returns NotFound (404) when the ID does not exist.
        /// </summary>
        [Fact]
        public async Task GivenNonExistentId_WhenGetById_ThenReturnsNotFound()
        {
            // arrange
            var id = Guid.NewGuid();
            this.userService.Setup(x => x.GetByIdAsync(id)).ReturnsAsync((UserProfileDto?)null);

            var controller = new UsersController(this.userService.Object, this.logger.Object);

            // act
            var result = await controller.GetById(id);

            // assert
            Assert.IsType<NotFoundResult>(result);
            this.userService.Verify(x => x.GetByIdAsync(id), Times.Once);
        }

        /// <summary>
        /// Verifies that GetByUserId returns OK (200) with user profile when a valid user ID is provided.
        /// </summary>
        [Fact]
        public async Task GivenValidUserId_WhenGetByUserId_ThenReturnsOk()
        {
            // arrange
            var userId = Guid.NewGuid();
            var profile = new UserProfileDto { Id = Guid.NewGuid(), UserId = userId, FirstName = "Test" };

            this.userService.Setup(x => x.GetByUserIdAsync(userId)).ReturnsAsync(profile);

            var controller = new UsersController(this.userService.Object, this.logger.Object);

            // act
            var result = await controller.GetByUserId(userId);

            // assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.OK, okResult.StatusCode);
            this.userService.Verify(x => x.GetByUserIdAsync(userId), Times.Once);
        }

        /// <summary>
        /// Verifies that DebitWallet returns OK (200) with updated balance when valid data is provided.
        /// </summary>
        [Fact]
        public async Task GivenValidData_WhenDebitWallet_ThenReturnsOk()
        {
            // arrange
            var id = Guid.NewGuid();
            var dto = new WalletOperationDto { Amount = 100 };

            this.userService.Setup(x => x.DebitWalletAsync(id, dto.Amount)).ReturnsAsync(400m);

            var controller = new UsersController(this.userService.Object, this.logger.Object);

            // act
            var result = await controller.DebitWallet(id, dto);

            // assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.OK, okResult.StatusCode);
            this.userService.Verify(x => x.DebitWalletAsync(id, dto.Amount), Times.Once);
        }

        /// <summary>
        /// Verifies that DebitWallet returns BadRequest (400) when the amount is invalid (negative).
        /// </summary>
        [Fact]
        public async Task GivenInvalidAmount_WhenDebitWallet_ThenReturnsBadRequest()
        {
            // arrange
            var id = Guid.NewGuid();
            var dto = new WalletOperationDto { Amount = -10 };

            var controller = new UsersController(this.userService.Object, this.logger.Object);

            // act
            var result = await controller.DebitWallet(id, dto);

            // assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.BadRequest, badRequestResult.StatusCode);
            this.userService.Verify(x => x.DebitWalletAsync(It.IsAny<Guid>(), It.IsAny<decimal>()), Times.Never);
        }

        /// <summary>
        /// Verifies that DebitWallet returns NotFound (404) when the user does not exist.
        /// </summary>
        [Fact]
        public async Task GivenNonExistentUser_WhenDebitWallet_ThenReturnsNotFound()
        {
            // arrange
            var id = Guid.NewGuid();
            var dto = new WalletOperationDto { Amount = 100 };

            this.userService.Setup(x => x.DebitWalletAsync(id, dto.Amount)).ThrowsAsync(new KeyNotFoundException());

            var controller = new UsersController(this.userService.Object, this.logger.Object);

            // act
            var result = await controller.DebitWallet(id, dto);

            // assert
            Assert.IsType<NotFoundResult>(result);
            this.userService.Verify(x => x.DebitWalletAsync(id, dto.Amount), Times.Once);
        }

        /// <summary>
        /// Verifies that DebitWallet returns Conflict (409) when the user has insufficient balance.
        /// </summary>
        [Fact]
        public async Task GivenInsufficientBalance_WhenDebitWallet_ThenReturnsConflict()
        {
            // arrange
            var id = Guid.NewGuid();
            var dto = new WalletOperationDto { Amount = 1000 };

            this.userService.Setup(x => x.DebitWalletAsync(id, dto.Amount))
                .ThrowsAsync(new InvalidOperationException("Insufficient balance"));

            var controller = new UsersController(this.userService.Object, this.logger.Object);

            // act
            var result = await controller.DebitWallet(id, dto);

            // assert
            var conflictResult = Assert.IsType<ConflictObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.Conflict, conflictResult.StatusCode);
            this.userService.Verify(x => x.DebitWalletAsync(id, dto.Amount), Times.Once);
        }

        /// <summary>
        /// Verifies that CreditWallet returns OK (200) with updated balance when valid data is provided.
        /// </summary>
        [Fact]
        public async Task GivenValidData_WhenCreditWallet_ThenReturnsOk()
        {
            // arrange
            var id = Guid.NewGuid();
            var dto = new WalletOperationDto { Amount = 100 };

            this.userService.Setup(x => x.CreditWalletAsync(id, dto.Amount)).ReturnsAsync(600m);

            var controller = new UsersController(this.userService.Object, this.logger.Object);

            // act
            var result = await controller.CreditWallet(id, dto);

            // assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.OK, okResult.StatusCode);
            this.userService.Verify(x => x.CreditWalletAsync(id, dto.Amount), Times.Once);
        }

        /// <summary>
        /// Verifies that PhoneNumberExists returns OK (200) when checking an existing phone number.
        /// </summary>
        [Fact]
        public async Task GivenValidPhoneNumber_WhenPhoneNumberExists_ThenReturnsOk()
        {
            // arrange
            var phoneNumber = "+1234567890";
            this.userService.Setup(x => x.PhoneNumberExistsAsync(phoneNumber)).ReturnsAsync(true);

            var controller = new UsersController(this.userService.Object, this.logger.Object);

            // act
            var result = await controller.PhoneNumberExists(phoneNumber);

            // assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.OK, okResult.StatusCode);
            this.userService.Verify(x => x.PhoneNumberExistsAsync(phoneNumber), Times.Once);
        }

        /// <summary>
        /// Verifies that Create returns Created (201) with user profile when valid data is provided.
        /// </summary>
        [Fact]
        public async Task GivenValidData_WhenCreate_ThenReturnsCreated()
        {
            // arrange
            var dto = new CreateUserDto { UserId = Guid.NewGuid(), FirstName = "Test", PhoneNumber = "+1234567890" };
            var created = new UserProfileDto { Id = Guid.NewGuid(), UserId = dto.UserId, FirstName = dto.FirstName };

            this.userService.Setup(x => x.CreateAsync(dto)).ReturnsAsync(created);

            var controller = new UsersController(this.userService.Object, this.logger.Object);

            // act
            var result = await controller.Create(dto);

            // assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal((int)HttpStatusCode.Created, createdResult.StatusCode);
            this.userService.Verify(x => x.CreateAsync(dto), Times.Once);
        }

        /// <summary>
        /// Verifies that Create returns BadRequest (400) when the user ID is empty.
        /// </summary>
        [Fact]
        public async Task GivenEmptyUserId_WhenCreate_ThenReturnsBadRequest()
        {
            // arrange
            var dto = new CreateUserDto { UserId = Guid.Empty, FirstName = "Test" };
            var controller = new UsersController(this.userService.Object, this.logger.Object);

            // act
            var result = await controller.Create(dto);

            // assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.BadRequest, badRequestResult.StatusCode);
            this.userService.Verify(x => x.CreateAsync(It.IsAny<CreateUserDto>()), Times.Never);
        }

        /// <summary>
        /// Verifies that Update returns OK (200) with updated profile when valid data is provided.
        /// </summary>
        [Fact]
        public async Task GivenValidData_WhenUpdate_ThenReturnsOk()
        {
            // arrange
            var id = Guid.NewGuid();
            var dto = new CreateUserDto { UserId = Guid.NewGuid(), FirstName = "Updated" };
            var updated = new UserProfileDto { Id = id, UserId = dto.UserId, FirstName = dto.FirstName };

            this.userService.Setup(x => x.UpdateAsync(id, dto)).ReturnsAsync(updated);

            var controller = new UsersController(this.userService.Object, this.logger.Object);

            // act
            var result = await controller.Update(id, dto);

            // assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.OK, okResult.StatusCode);
            this.userService.Verify(x => x.UpdateAsync(id, dto), Times.Once);
        }

        /// <summary>
        /// Verifies that Update returns NotFound (404) when the ID does not exist.
        /// </summary>
        [Fact]
        public async Task GivenNonExistentId_WhenUpdate_ThenReturnsNotFound()
        {
            // arrange
            var id = Guid.NewGuid();
            var dto = new CreateUserDto { UserId = Guid.NewGuid(), FirstName = "Updated" };

            this.userService.Setup(x => x.UpdateAsync(id, dto)).ReturnsAsync((UserProfileDto?)null);

            var controller = new UsersController(this.userService.Object, this.logger.Object);

            // act
            var result = await controller.Update(id, dto);

            // assert
            Assert.IsType<NotFoundResult>(result);
            this.userService.Verify(x => x.UpdateAsync(id, dto), Times.Once);
        }

        /// <summary>
        /// Verifies that AddBalance returns OK (200) with updated balance when valid data is provided.
        /// </summary>
        [Fact]
        public async Task GivenValidData_WhenAddBalance_ThenReturnsOk()
        {
            // arrange
            var userId = Guid.NewGuid();
            var dto = new AddBalanceDto { UserId = userId, Amount = 500 };
            var profile = new UserProfileDto { Id = Guid.NewGuid(), UserId = userId };

            this.userService.Setup(x => x.GetByUserIdAsync(userId)).ReturnsAsync(profile);
            this.userService.Setup(x => x.CreditWalletAsync(profile.Id, dto.Amount)).ReturnsAsync(1000m);

            var controller = new UsersController(this.userService.Object, this.logger.Object);

            // act
            var result = await controller.AddBalance(dto);

            // assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.OK, okResult.StatusCode);
            this.userService.Verify(x => x.GetByUserIdAsync(userId), Times.Once);
            this.userService.Verify(x => x.CreditWalletAsync(profile.Id, dto.Amount), Times.Once);
        }

        /// <summary>
        /// Verifies that AddBalance returns BadRequest (400) when the user ID is empty.
        /// </summary>
        [Fact]
        public async Task GivenEmptyUserId_WhenAddBalance_ThenReturnsBadRequest()
        {
            // arrange
            var dto = new AddBalanceDto { UserId = Guid.Empty, Amount = 500 };
            var controller = new UsersController(this.userService.Object, this.logger.Object);

            // act
            var result = await controller.AddBalance(dto);

            // assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.BadRequest, badRequestResult.StatusCode);
        }
    }
}
