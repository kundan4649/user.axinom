namespace z5.ms.common.infrastructure.id
{
    /// <summary>
    /// Common interface for different password hashing and verification strategies
    /// </summary>
    public interface IPasswordEncryptionStrategy
    {
        /// <summary>Hash a password</summary>
        /// <param name="password">Password to hash</param>
        /// <returns>Hashed password</returns>
        /// <remarks>
        /// The format of the resulting string is strategy specific: it may contain multiple fields e.g. hash + salt
        /// </remarks>
        string HashPassword(string password);

        /// <summary>Verify a password</summary>
        /// <param name="password">Password to verify</param>
        /// <param name="hash">The hash the password should be verified against</param>
        /// <returns>True if the the passwords match, false otherwise</returns>
        /// <remarks>
        /// The implementing strategy has to be aware of the hash format i.e. it should be able to correctly 
        /// parse the hash string and extract all relevant info, e.g. hash + salt
        /// </remarks>
        bool VerifyPassword(string password, string hash);
        
        // TODO: create a method for generating password forgotten/reset tokens
    }
}