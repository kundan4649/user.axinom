using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Management;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using z5.ms.common.abstractions;
using z5.ms.common.extensions;

namespace z5.ms.common.infrastructure.events
{
    /// <summary>Publishes events of a specified type to an event broker.</summary>
    public interface IEventPublisher<T> : IPublisher<T>
    {
    }

    /// <summary>
    ///  Uses an azure service bus topic as an event broker.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class EventPublisher<T> : IEventPublisher<T>
    {
        private readonly EventBusOptions _settings;
        private readonly ManagementClient _managementClient;
        private ITopicClient _topicClient;
        private readonly string _topicName;
        private readonly ILogger _logger;

        /// <summary>
        /// A retry policy to be applied to messages when message handling results in an exception.
        /// </summary>
        public RetryPolicy RetryPolicy => new RetryExponential(TimeSpan.FromMinutes(1), TimeSpan.FromMinutes(60), 3);

        /// <inheritdoc />
        public EventPublisher(IOptions<EventBusOptions> options, ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger(GetType().Name);
            _topicName = $"{typeof(T).Name.ToLower()}s";
            _settings = options.Value;
            _managementClient = new ManagementClient(_settings.MsEventBusConnection);
            InitialiseTopicClient();
        }

        private void InitialiseTopicClient()
        {
            _logger.LogInformation($"Initialising service bus topic: {_settings.MsEventBusName} . {_topicName}");
            _managementClient.CreateTopicIfNotExists(new TopicDescription(_topicName)).Wait();

            _topicClient = new TopicClient(
                _settings.MsEventBusConnection,
                _topicName,
                RetryPolicy);
        }

        /// <summary>
        /// Publish a message.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task<Result<Success>> Publish(T message)
        {
            var json = JsonConvert.SerializeObject(message);
            var sbMessage = new Message(Encoding.UTF8.GetBytes(json)) { ContentType = "application/json" };
            await _topicClient.SendAsync(sbMessage);
            _logger.LogDebug($"{typeof(T).Name} published: {json}");
            return Result<Success>.FromValue(new Success { Code = 0, Message = $"{typeof(T).Name} published" });
        }

        /// <inheritdoc />
        public void Dispose()
        {
            _topicClient?.CloseAsync().Wait();
            GC.SuppressFinalize(this);
        }
    }
}