using BCrypt.Net;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Net.Http.Json;

namespace AuthService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly IJwtService _jwt;
    private readonly ILogger<AuthController> _logger;
    private readonly IHttpClientFactory _httpClientFactory;

    public AuthController(AppDbContext db, IJwtService jwt, ILogger<AuthController> logger, IHttpClientFactory httpClientFactory)
    {
        _db = db;
        _jwt = jwt;
        _logger = logger;
        _httpClientFactory = httpClientFactory;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        // Backend validation
        if (string.IsNullOrWhiteSpace(dto.FullName))
            return BadRequest(new { error = "Full name is required" });
        if (string.IsNullOrWhiteSpace(dto.Email))
            return BadRequest(new { error = "Email is required" });
        if (!System.Text.RegularExpressions.Regex.IsMatch(dto.Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            return BadRequest(new { error = "Invalid email format" });
        if (string.IsNullOrWhiteSpace(dto.Password))
            return BadRequest(new { error = "Password is required" });
        if (string.IsNullOrWhiteSpace(dto.ConfirmPassword))
            return BadRequest(new { error = "Confirm password is required" });
        if (dto.Password != dto.ConfirmPassword)
            return BadRequest(new { error = "Passwords do not match" });
        if (!System.Text.RegularExpressions.Regex.IsMatch(dto.Password, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^A-Za-z\d]).{8,}$"))
            return BadRequest(new { error = "Password must be 8+ chars, include upper, lower, number, special" });

        // Validate phone number
        if (string.IsNullOrWhiteSpace(dto.PhoneNumber))
            return BadRequest(new { error = "Phone number is required" });
        if (!System.Text.RegularExpressions.Regex.IsMatch(dto.PhoneNumber, @"^\+?\d{10,15}$"))
            return BadRequest(new { error = "Invalid phone number format (10-15 digits, optional + prefix)" });

        try
        {
            // Check for duplicate email
            var emailExists = await _db.Users.AnyAsync(u => u.Email == dto.Email);
            if (emailExists) return Conflict(new { error = "Email already registered" });

            // Check for duplicate phone number via User Service - MANDATORY check
            try
            {
                var httpClient = _httpClientFactory.CreateClient("user");
                var phoneCheckResponse = await httpClient.GetAsync($"/api/users/phone-exists/{Uri.EscapeDataString(dto.PhoneNumber)}");
                if (phoneCheckResponse.IsSuccessStatusCode)
                {
                    var phoneCheckResult = await phoneCheckResponse.Content.ReadFromJsonAsync<PhoneExistsResponse>();
                    if (phoneCheckResult?.Exists == true)
                        return Conflict(new { error = "Phone number already registered" });
                }
                else
                {
                    _logger.LogError("User Service returned non-success status {StatusCode} when checking phone number", phoneCheckResponse.StatusCode);
                    return StatusCode(503, new { error = "Unable to validate phone number. Please try again later." });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to check phone number existence in User Service. Registration aborted.");
                return StatusCode(503, new { error = "Unable to validate phone number. Please try again later." });
            }

            // Password is validated, hash it before storing
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            var user = new User
            {
                Email = dto.Email,
                PasswordHash = passwordHash,
                FullName = dto.FullName
            };

            _db.Users.Add(user);
            await _db.SaveChangesAsync();

            // Now create user profile in User Service - if this fails, we need to rollback
            try
            {
                var names = dto.FullName.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                var firstName = names.Length > 0 ? names[0] : "";
                var lastName = names.Length > 1 ? string.Join(" ", names.Skip(1)) : "";

                var httpClient = _httpClientFactory.CreateClient("user");
                var profileDto = new
                {
                    UserId = user.Id,
                    FirstName = firstName,
                    LastName = lastName,
                    PhoneNumber = dto.PhoneNumber,
                    Address = dto.Address
                };

                var profileResponse = await httpClient.PostAsJsonAsync("/api/users", profileDto);
                
                if (!profileResponse.IsSuccessStatusCode)
                {
                    // Profile creation failed - rollback auth user creation
                    _db.Users.Remove(user);
                    await _db.SaveChangesAsync();
                    
                    var errorContent = await profileResponse.Content.ReadAsStringAsync();
                    _logger.LogError("Profile creation failed with status {StatusCode}: {Error}. Auth user rolled back.", profileResponse.StatusCode, errorContent);
                    
                    if (profileResponse.StatusCode == System.Net.HttpStatusCode.Conflict)
                    {
                        try
                        {
                            var errorObj = await profileResponse.Content.ReadFromJsonAsync<System.Text.Json.JsonElement>();
                            if (errorObj.TryGetProperty("error", out var errorProp))
                            {
                                return Conflict(new { error = errorProp.GetString() });
                            }
                        }
                        catch { }
                        return Conflict(new { error = "Phone number already registered" });
                    }
                    
                    return StatusCode((int)profileResponse.StatusCode, new { error = "Failed to create user profile. Registration aborted." });
                }
            }
            catch (Exception ex)
            {
                // Profile creation failed - rollback auth user creation
                _db.Users.Remove(user);
                await _db.SaveChangesAsync();
                _logger.LogError(ex, "Profile creation failed. Auth user rolled back for {Email}", dto.Email);
                return StatusCode(500, new { error = "Failed to create user profile. Registration aborted." });
            }

            return CreatedAtAction(nameof(Me), new { id = user.Id }, new { user.Id, user.Email, user.FullName });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Registration failed for {Email}", dto.Email);
            return StatusCode(500, new { error = "Registration failed. Please try again later." });
        }
    }

    private class PhoneExistsResponse
    {
        public bool Exists { get; set; }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Email) || string.IsNullOrWhiteSpace(dto.Password))
            return BadRequest(new { error = "Email and password required" });

        var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
        if (user == null) return Unauthorized(new { error = "Invalid credentials" });

        var valid = BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash);
        if (!valid) return Unauthorized(new { error = "Invalid credentials" });

        var token = _jwt.GenerateToken(user);
        return Ok(new AuthResponseDto { Token = token, ExpiresIn = 6 * 60 * 60, UserId = user.Id, Email = user.Email });
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword(ResetPasswordDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Email) || string.IsNullOrWhiteSpace(dto.NewPassword))
            return BadRequest(new { error = "Email and new password required" });

        var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
        if (user == null) return NotFound(new { error = "User not found" });

        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);
        await _db.SaveChangesAsync();

        return Ok(new { status = "password reset" });
    }

    [Authorize]
    [HttpGet("me")]
    public async Task<IActionResult> Me()
    {
        var id = User.FindFirst(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (id == null) return Unauthorized();
        if (!Guid.TryParse(id, out var guid)) return Unauthorized();

        var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == guid);
        if (user == null) return NotFound();

        return Ok(new { user.Id, user.Email, user.FullName, user.CreatedAt });
    }
}

public record RegisterDto(
    string Email,
    string Password,
    string ConfirmPassword,
    string? FullName,
    string? PhoneNumber,
    string? Address
);
public record LoginDto(string Email, string Password);
public record ResetPasswordDto(string Email, string NewPassword);
public class AuthResponseDto
{
    public string Token { get; set; } = null!;
    public int ExpiresIn { get; set; }
    public Guid UserId { get; set; }
    public string Email { get; set; } = null!;
}
