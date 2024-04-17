using System;
using System.Threading.Tasks;

namespace z5.ms.common.abstractions
{
    /// <inheritdoc />
    /// <summary>A generic interface for publishing events or commands</summary>
    public interface IPublisher<in T> : IDisposable
    {
        /// <summary>Publish a message</summary>
        /// <param name="message"></param>
        /// <returns></returns>
        Task<Result<Success>> Publish(T message);
    }
}