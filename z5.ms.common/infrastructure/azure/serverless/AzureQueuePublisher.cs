using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using z5.ms.common.abstractions;

namespace z5.ms.common.infrastructure.azure.serverless
{
    /// <summary>Create an IPublisher wrapper around an existing IAsyncCollector instance</summary>
    public class AzureQueuePublisher<T> : IPublisher<T>
    {
        private readonly IAsyncCollector<T> _collector;

        /// <inheritdoc />
        public AzureQueuePublisher(IAsyncCollector<T> collector) => _collector = collector;

        /// <inheritdoc />
        public async Task<Result<Success>> Publish(T message)
        {
            await _collector.AddAsync(message);
            return new Result<Success>();
        }

        /// <inheritdoc />
        public void Dispose() => _collector.FlushAsync().Wait();
    }

    /// <summary>IAsyncCollector extensions</summary>
    public static class CollectorExtensions
    {
        /// <summary>Create an IPublisher wrapper around an existing IAsyncCollector instance</summary>
        public static IPublisher<T> ToIPublisher<T>(this IAsyncCollector<T> collector) 
            => new AzureQueuePublisher<T>(collector);
    }
}
