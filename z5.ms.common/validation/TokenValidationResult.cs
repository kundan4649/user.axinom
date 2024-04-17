namespace z5.ms.common.validation
{
    /// <summary>
    /// Token validation result
    /// </summary>
    public class TokenValidationResult : ValidationResult
    {
        /// <summary>Authentication token</summary>
        public AuthToken Token { get; set; }

        /// <summary>
        /// Constructor from token value
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public TokenValidationResult(AuthToken token)
        {
            Token = token;
        }

        /// <inheritdoc />
        public TokenValidationResult(string message, int code = 401) : base(message, code)
        {
        }
    }
}