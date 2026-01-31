// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AuthController.cs" company="Electronic-Paradise">
//   © Electronic-Paradise. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Net;
using System.Security.Claims;

using AuthService.Abstraction.DTOs.Requests;
using AuthService.Abstraction.DTOs.Responses;
using AuthService.Core.Business;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.API.Controllers;

/// <summary>
/// Provides authentication endpoints for user registration, login, password reset, and profile retrieval.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly ILogger<AuthController> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="AuthController"/> class.
    /// </summary>
    /// <param name="authService">The authentication service.</param>
    /// <param name="logger">The logger instance.</param>
    public AuthController(
        IAuthService authService,
        ILogger<AuthController> logger)
    {
        _authService = authService ?? throw new ArgumentNullException(nameof(authService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Registers a new user account.
    /// </summary>
    /// <param name="request">The registration request.</param>
    /// <returns>An <see cref="IActionResult"/> containing the created user information or error details.</returns>
    /// <response code="201">Returns the newly created user with authentication token.</response>
    /// <response code="400">If the registration data is invalid.</response>
    /// <response code="409">If the email or phone number already exists.</response>
    /// <response code="500">If an internal server error occurs.</response>
    /// <response code="503">If the user service is unavailable.</response>
    [HttpPost("register")]
    [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        // NOTE: Most validation is now handled by [Required] and [RegularExpression] attributes on RegisterRequest
        // This is a legacy controller with custom validation logic - should be cleaned up in future iteration
        try
        {
            // Register user (service returns AuthResponse with token)
            var authResponse = await _authService.RegisterAsync(request);

            return CreatedAtAction(nameof(Me), null, authResponse);
        }
        catch (InvalidOperationException ex) when (ex.Message == "EMAIL_EXISTS")
        {
            return Conflict(new { error = "Email already registered" });
        }
        catch (InvalidOperationException ex) when (ex.Message == "PHONE_EXISTS")
        {
            return Conflict(new { error = "Phone number already registered" });
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.ServiceUnavailable)
        {
            return StatusCode(503, new { error = "User service unavailable. Please try again." });
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.Conflict)
        {
            // Downstream user-service duplicate validation etc.
            return Conflict(new { error = "User profile creation failed due to a conflict" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Registration failed for {Email}", request.Email);
            return StatusCode(500, new { error = "Registration failed. Please try again later." });
        }
    }

    /// <summary>
    /// Authenticates a user and returns a JWT token.
    /// </summary>
    /// <param name="request">The login request containing email and password.</param>
    /// <returns>An <see cref="IActionResult"/> containing the authentication token or error details.</returns>
    /// <response code="200">Returns the authentication token and user information.</response>
    /// <response code="400">If the login data is invalid.</response>
    /// <response code="401">If the credentials are invalid.</response>
    [HttpPost("login")]
    [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        try
        {
            var authResponse = await _authService.LoginAsync(request);
            return Ok(authResponse);
        }
        catch (KeyNotFoundException ex) when (ex.Message == "USER_NOT_FOUND")
        {
            return NotFound(new { code = "USER_NOT_FOUND", error = "User not found" });
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Login failed for {Email}", request.Email);
            return StatusCode(500, new { error = "Login failed. Please try again." });
        }
    }

    /// <summary>
    /// Resets a user's password.
    /// </summary>
    /// <param name="request">The reset password request containing email, token, and new password.</param>
    /// <returns>An <see cref="IActionResult"/> indicating the result of the password reset operation.</returns>
    /// <response code="200">If the password was reset successfully.</response>
    /// <response code="400">If the reset data is invalid.</response>
    /// <response code="404">If the user was not found.</response>
    [HttpPost("reset-password")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ResetPassword(ResetPasswordRequest request)
    {
        var success = await _authService.ResetPasswordAsync(request);
        if (!success)
        {
            return NotFound(new { error = "User not found or invalid reset token" });
        }

        return Ok(new { status = "password reset" });
    }

    /// <summary>
    /// Retrieves the authenticated user's profile information.
    /// </summary>
    /// <returns>An <see cref="IActionResult"/> containing the user's profile information.</returns>
    /// <response code="200">Returns the user's profile information.</response>
    /// <response code="401">If the user is not authenticated.</response>
    /// <response code="404">If the user was not found.</response>
    [Authorize]
    [HttpGet("me")]
    [ProducesResponseType(typeof(UserDetailResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Me()
    {
        var id = User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        if (id == null)
        {
            return Unauthorized();
        }

        if (!Guid.TryParse(id, out var guid))
        {
            return Unauthorized();
        }

        var user = await _authService.GetUserByIdAsync(guid);
        if (user == null)
        {
            return NotFound();
        }

        return Ok(user);
    }
}
