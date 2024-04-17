using System;

namespace z5.ms.common.abstractions
{
    /// <summary> Exception type for not found cases </summary>
    public class NotFoundException : Exception
    {
        /// <inheritdoc />
        public NotFoundException(string message) : base(message){}
    }
}