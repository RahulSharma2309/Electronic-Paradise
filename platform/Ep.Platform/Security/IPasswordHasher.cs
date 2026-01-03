namespace Ep.Platform.Security;

/// <summary>
/// Abstracts password hashing to avoid direct BCrypt dependency in business layer
/// </summary>
public interface IPasswordHasher
{
    /// <summary>
    /// Hash a plaintext password
    /// </summary>
    string HashPassword(string password);

    /// <summary>
    /// Verify a plaintext password against a hash
    /// </summary>
    bool VerifyPassword(string password, string hash);
}





