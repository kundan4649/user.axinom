using System;

namespace z5.ms.domain.user.exceptions
{
    /// <summary>
    /// Common exception type for all kind of internal errors
    /// </summary>
    public abstract class CoreException : Exception
    {
        /// <inheritdoc />
        protected CoreException(int errorCode, string message) : base($"Assertion not met ({errorCode}): {message}")
        {
            ErrorCode = errorCode;
        }

        /// <summary>Error code</summary>
        public int ErrorCode { get; }
    }
}
