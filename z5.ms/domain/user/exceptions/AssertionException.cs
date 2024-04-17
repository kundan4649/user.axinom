namespace z5.ms.domain.user.exceptions
{
    /// <summary>
    /// Exception type for assertion errors
    /// </summary>
    public class AssertionException : CoreException
    {
        /// <inheritdoc />
        public AssertionException(int errorCode, string message) : base(errorCode, message)
        {
        }
    }
}
