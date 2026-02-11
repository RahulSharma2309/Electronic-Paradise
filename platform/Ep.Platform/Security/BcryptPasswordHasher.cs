namespace Ep.Platform.Security
{
    /// <summary>
    /// BCrypt implementation of password hashing.
    /// </summary>
    public class BcryptPasswordHasher : IPasswordHasher
    {
        /// <inheritdoc />
        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        /// <inheritdoc />
        public bool VerifyPassword(string password, string hash)
        {
            return BCrypt.Net.BCrypt.Verify(password, hash);
        }
    }
}
