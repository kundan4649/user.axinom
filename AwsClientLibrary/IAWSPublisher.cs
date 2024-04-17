using System;
using System.Threading.Tasks;

namespace AwsClientLibrary
{
    /// <summary>A generic interface for publishing events or commands</summary>
    public interface IAWSPublisher<in T>
    {
        /// <summary>Publish a message</summary>
        /// <param name="message"></param>
        /// <returns></returns>
        Task Publish(T message);
    }
}
