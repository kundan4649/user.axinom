using z5.ms.common.abstractions;

namespace z5.ms.common.validation
{
    /// <summary>
    /// Validation result
    /// </summary>
    public class ValidationResult
    {
        /// <summary>Bool result IsValid</summary>
        public bool IsValid { get; set; }

        /// <summary>Validation error</summary>
        public Error Error { get; set; }

        /// <summary>Create valid result</summary>
        public ValidationResult()
        {
            IsValid = true;
        }

        /// <summary>Create error result</summary>
        public ValidationResult(string message, int code = 401)
        {
            IsValid = false;
            Error = new Error { Code = code, Message = message };
        }
    }
}