using System.Threading.Tasks;
using z5.ms.common.abstractions;

namespace z5.ms.common.infrastructure.events
{
    /// <summary>
    /// Handles published events of a specified type.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IEventHandler<T>
    {
        /// <summary>
        /// Handle the event
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        Task<Result<Success>> Handle(T message);

        /// <summary> Message routing mode determines how messages are handled when multiple instances of a service are deployed </summary>
        EventRoutingMode EventRoutingMode { get; }
    }

    /// <summary>
    /// Message routing mode determines how messages are handled when multiple instances of a service are deployed
    /// </summary>
    public enum EventRoutingMode
    {
        /// <summary>Each service instance will handle every published message of this type. </summary>
        Fanout,

        /// <summary>Multiple instances of a service will compete to handle each message. </summary>
        RoundRobin
    }
}