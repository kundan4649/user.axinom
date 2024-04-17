namespace z5.ms.domain.user.exceptions
{
    /// <summary>
    /// Exception type for item not found errors
    /// </summary>
    public class ItemNotFoundException : CoreException
    {
        /// <inheritdoc />
        public ItemNotFoundException(int errorCode, string message) : base(errorCode, message)
        {
        }
    }
}
