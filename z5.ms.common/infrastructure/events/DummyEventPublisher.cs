using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using z5.ms.common.abstractions;

namespace z5.ms.common.infrastructure.events
{
    /// <summary>
    ///  A dummy event publisher that does nothing on publish.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// TODO: Remove this once all events are being published to bus
    public class DummyEventPublisher<T> : IEventPublisher<T>
    {
        private readonly ILogger _logger;

        /// <inheritdoc />
        public DummyEventPublisher(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger(GetType().Name);
        }

        /// <summary>
        /// Publish a message.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task<Result<Success>> Publish(T message)
        {
            _logger.LogInformation($"{typeof(T).Name} raised to DummyEventPublisher. No action taken.");
            await Task.CompletedTask;
            return new Result<Success>();
        }

        /// <inheritdoc />
        public void Dispose()
        {
        }
    }
}