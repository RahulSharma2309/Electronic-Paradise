// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UsersController.cs" company="Electronic-Paradise">
//   © Electronic-Paradise. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using Microsoft.AspNetCore.Mvc;
using UserService.Abstraction.DTOs.Requests;
using UserService.Abstraction.DTOs.Responses;
using UserService.Core.Business;

namespace UserService.API.Controllers;

/// <summary>
/// Provides endpoints for user profile management and wallet operations.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _service;
    private readonly ILogger<UsersController> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="UsersController"/> class.
    /// </summary>
    /// <param name="service">The user service for business logic operations.</param>
    /// <param name="logger">The logger instance for structured logging.</param>
    public UsersController(IUserService service, ILogger<UsersController> logger)
    {
        _service = service ?? throw new ArgumentNullException(nameof(service));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Debits (subtracts) an amount from a user's wallet.
    /// </summary>
    /// <param name="userId">The user ID from the authentication service.</param>
    /// <param name="request">The wallet operation request containing the amount.</param>
    /// <returns>An <see cref="IActionResult"/> containing the wallet balance response or error details.</returns>
    /// <response code="200">Returns the wallet balance response.</response>
    /// <response code="400">If the amount is invalid.</response>
    /// <response code="404">If the user is not found.</response>
    /// <response code="409">If there is insufficient wallet balance.</response>
    [HttpPost("{userId:guid}/wallet/debit")]
    [ProducesResponseType(typeof(WalletBalanceResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> DebitWallet(Guid userId, [FromBody] WalletOperationRequest request)
    {
        try
        {
            var response = await _service.DebitWalletAsync(userId, request.Amount);
            return Ok(response);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { error = ex.Message });
        }
    }

    /// <summary>
    /// Credits (adds) an amount to a user's wallet.
    /// </summary>
    /// <param name="userId">The user ID from the authentication service.</param>
    /// <param name="request">The wallet operation request containing the amount.</param>
    /// <returns>An <see cref="IActionResult"/> containing the wallet balance response or error details.</returns>
    /// <response code="200">Returns the wallet balance response.</response>
    /// <response code="400">If the amount is invalid.</response>
    /// <response code="404">If the user is not found.</response>
    [HttpPost("{userId:guid}/wallet/credit")]
    [ProducesResponseType(typeof(WalletBalanceResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CreditWallet(Guid userId, [FromBody] WalletOperationRequest request)
    {
        try
        {
            var response = await _service.CreditWalletAsync(userId, request.Amount);
            return Ok(response);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { error = ex.Message });
        }
    }

    /// <summary>
    /// Retrieves a user profile by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the user profile.</param>
    /// <returns>An <see cref="IActionResult"/> containing the user profile or not found status.</returns>
    /// <response code="200">Returns the user profile.</response>
    /// <response code="404">If the user profile is not found.</response>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(UserProfileDetailResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var res = await _service.GetByIdAsync(id);
        if (res == null)
        {
            return NotFound();
        }

        return Ok(res);
    }

    /// <summary>
    /// Retrieves a user profile by the authentication service user ID.
    /// </summary>
    /// <param name="userId">The user ID from the authentication service.</param>
    /// <returns>An <see cref="IActionResult"/> containing the user profile or not found status.</returns>
    /// <response code="200">Returns the user profile.</response>
    /// <response code="404">If the user profile is not found.</response>
    [HttpGet("by-userid/{userId:guid}")]
    [ProducesResponseType(typeof(UserProfileResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByUserId(Guid userId)
    {
        var res = await _service.GetByUserIdAsync(userId);
        if (res == null)
        {
            return NotFound();
        }

        return Ok(res);
    }

    /// <summary>
    /// Checks whether a phone number is already registered.
    /// </summary>
    /// <param name="phoneNumber">The phone number to check.</param>
    /// <returns>An <see cref="IActionResult"/> containing the existence status.</returns>
    /// <response code="200">Returns whether the phone number exists.</response>
    [HttpGet("phone-exists/{phoneNumber}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> PhoneNumberExists(string phoneNumber)
    {
        var exists = await _service.PhoneNumberExistsAsync(phoneNumber);
        return Ok(new { exists });
    }

    /// <summary>
    /// Adds balance to a user's wallet using their authentication service user ID.
    /// </summary>
    /// <param name="userId">The user ID from the authentication service.</param>
    /// <param name="request">The add balance request containing the amount.</param>
    /// <returns>An <see cref="IActionResult"/> containing the wallet balance response or error details.</returns>
    /// <response code="200">Returns the wallet balance response.</response>
    /// <response code="400">If the request data is invalid.</response>
    /// <response code="404">If the user profile is not found.</response>
    /// <response code="500">If an internal server error occurs.</response>
    [HttpPost("{userId:guid}/add-balance")]
    [ProducesResponseType(typeof(WalletBalanceResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AddBalance(Guid userId, [FromBody] AddBalanceRequest request)
    {
        try
        {
            var response = await _service.AddBalanceAsync(userId, request);
            return Ok(response);
        }
        catch (KeyNotFoundException)
        {
            return NotFound(new { error = "User profile not found" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding balance for userId {UserId}", userId);
            return StatusCode(500, new { error = "Failed to add balance. Please try again." });
        }
    }

    /// <summary>
    /// Creates a new user profile.
    /// </summary>
    /// <param name="request">The user creation request.</param>
    /// <returns>An <see cref="IActionResult"/> containing the created user profile or error details.</returns>
    /// <response code="201">Returns the newly created user profile.</response>
    /// <response code="400">If the request data is invalid.</response>
    [HttpPost]
    [ProducesResponseType(typeof(UserProfileDetailResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create(CreateUserProfileRequest request)
    {
        try
        {
            var created = await _service.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }
        catch (ArgumentException ex)
        {
            // e.g. "Phone number already registered"
            return Conflict(new { error = ex.Message });
        }
    }

    /// <summary>
    /// Updates an existing user profile.
    /// </summary>
    /// <param name="id">The unique identifier of the user profile to update.</param>
    /// <param name="request">The user update request.</param>
    /// <returns>An <see cref="IActionResult"/> containing the updated user profile or not found status.</returns>
    /// <response code="200">Returns the updated user profile.</response>
    /// <response code="404">If the user profile is not found.</response>
    [HttpPatch("{id:guid}")]
    [ProducesResponseType(typeof(UserProfileDetailResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Update(Guid id, UpdateUserProfileRequest request)
    {
        try
        {
            var updated = await _service.UpdateAsync(id, request);
            if (updated == null)
            {
                return NotFound();
            }

            return Ok(updated);
        }
        catch (ArgumentException ex)
        {
            return Conflict(new { error = ex.Message });
        }
    }
}
