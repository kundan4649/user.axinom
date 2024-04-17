using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace z5.ms.common.infrastructure.events
{
    /// <inheritdoc />
    /// <summary>
    /// A dummy event subscription that does not connect to a message broker
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// TODO: Remove this once all events are being published to bus
    public class DummyEventSubscription<T> : IEventSubscription<T>
    {
        private readonly ILogger _logger;
        
        /// <inheritdoc />
        public DummyEventSubscription(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger(GetType().Name);
        }

        /// <inheritdoc />
        public async Task Initialize()
        {
            await Task.CompletedTask;
            _logger.LogInformation($"{typeof(T).Name}s are handled by DummyEventSubscription. No messages will be dequeued.");
        }
        
        /// <inheritdoc />
        public void Dispose()
        {
        }
    }
}