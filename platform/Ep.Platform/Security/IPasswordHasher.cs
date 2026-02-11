namespace Ep.Platform.Security
{
    /// <summary>
    /// Abstracts password hashing to avoid a direct BCrypt dependency in the business layer.
    /// </summary>
    public interface IPasswordHasher
    {
        /// <summary>
        /// Hashes a plaintext password.
        /// </summary>
        /// <param name="password">The plaintext password.</param>
        /// <returns>The hashed password.</returns>
        string HashPassword(string password);

        /// <summary>
        /// Verifies a plaintext password against a stored hash.
        /// </summary>
        /// <param name="password">The plaintext password.</param>
        /// <param name="hash">The stored password hash.</param>
        /// <returns><c>true</c> if the password matches; otherwise, <c>false</c>.</returns>
        bool VerifyPassword(string password, string hash);
    }
}
