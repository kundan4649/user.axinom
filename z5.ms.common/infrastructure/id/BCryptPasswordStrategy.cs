using BCrypt.Net;
using Microsoft.Extensions.Logging;

namespace z5.ms.common.infrastructure.id
{
    /// <inheritdoc />
    public class BCryptPasswordStrategy : IPasswordEncryptionStrategy
    {
        private readonly ILogger _logger;

        /// <inheritdoc />
        public BCryptPasswordStrategy(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger(GetType().Name);
        }

        /// <inheritdoc />
        public string HashPassword(string password)
            => BCrypt.Net.BCrypt.HashPassword(password);

        /// <inheritdoc />
        public bool VerifyPassword(string password, string hash)
        {
            try
            {
                return BCrypt.Net.BCrypt.Verify(password, hash);
            }
            catch (SaltParseException e)
            {
                _logger.LogError(e.Message);
                return false;
            }
            
        }
    }
}