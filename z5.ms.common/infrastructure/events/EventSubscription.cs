using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Management;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using z5.ms.common.extensions;

namespace z5.ms.common.infrastructure.events
{
    /// <summary>
    /// An event subscription connects an event handler with an event broker.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    // ReSharper disable once UnusedTypeParameter
    public interface IEventSubscription<T> : IDisposable
    {
        /// <summary>
        /// Initialize the event handler
        /// </summary>
        Task Initialize();
    }

    /// <inheritdoc />
    /// <summary>
    /// Uses an azure service bus topic as an event broker.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class EventSubscription<T> : IEventSubscription<T>
    {
        private readonly IEventHandler<T> _eventHandler;
        private readonly ManagementClient _managementClient;
        private readonly EventBusOptions _settings;
        private readonly string _topicName;
        private readonly string _subscriptionName;
        private readonly ILogger _logger;
        private ISubscriptionClient _subscriptionClient;

        /// <inheritdoc />
        public EventSubscription(IEventHandler<T> eventHandler, IOptions<EventBusOptions> options, ILoggerFactory loggerFactory)
        {
            _eventHandler = eventHandler;
            _settings = options.Value;
            _managementClient = new ManagementClient(_settings.MsEventBusConnection);
            _logger = loggerFactory.CreateLogger(GetType().Name);
            _topicName = $"{typeof(T).Name}s";
            _subscriptionName = eventHandler.EventRoutingMode == EventRoutingMode.Fanout
                ? $"{_settings.MsServiceName}-{ Environment.GetEnvironmentVariable("COMPONENT_HOST_NAME")?.ToLowerInvariant()}-{Environment.MachineName.ToLowerInvariant()}"
                : _settings.MsServiceName;
        }

        /// <inheritdoc />
        public async Task Initialize()
        {
            _logger.LogInformation($"Initialising service bus subscription: {_settings.MsEventBusName} . {_topicName} . {_subscriptionName}");
            await _managementClient.CreateTopicIfNotExists(new TopicDescription(_topicName));

            if (_eventHandler.EventRoutingMode == EventRoutingMode.Fanout)
                await _managementClient.DeleteSubscriptionIfExists(_topicName, _subscriptionName);

            await _managementClient.CreateSubscriptionIfNotExists(new SubscriptionDescription(_topicName, _subscriptionName));

            _subscriptionClient = new SubscriptionClient(
                _settings.MsEventBusConnection,
                _topicName,
                _subscriptionName,
                // ReSharper disable once RedundantArgumentDefaultValue
                ReceiveMode.PeekLock);

            var messageHandlerOptions = new MessageHandlerOptions(ExceptionReceivedHandler)
            {
                MaxConcurrentCalls = 1,
                AutoComplete = false
            };

            _subscriptionClient.RegisterMessageHandler(ProcessMessagesAsync, messageHandlerOptions);
        }

        /// <summary>Deserializes an incoming message and passes it to the handler.</summary>
        private async Task ProcessMessagesAsync(Message message, CancellationToken token)
        {
            // deserialize message
            T payload;
            try
            {
                var json = Encoding.UTF8.GetString(message.Body);
                _logger.LogInformation($"{typeof(T).Name} recieved: {json}");
                payload = JsonConvert.DeserializeObject<T>(json);
            }
            catch (Exception ex)
            {
                // deserialization exception.. do not retry
                _logger.LogError(ex.ToString());
                await _subscriptionClient.DeadLetterAsync(message.SystemProperties.LockToken);
                return;
            }

            // handle message
            var result = await _eventHandler.Handle(payload);
            if (!result.Success)
            {
                _logger.LogError($"{typeof(T).Name} handling failed: {result.Error.Message}");

                // TODO: investigate and implement limited retry policy
                await _subscriptionClient.AbandonAsync(message.SystemProperties.LockToken);
                return;
            }

            await _subscriptionClient.CompleteAsync(message.SystemProperties.LockToken);
        }

        /// <summary>Use this Handler to look at the exceptions received on the MessagePump</summary>
        private Task ExceptionReceivedHandler(ExceptionReceivedEventArgs exceptionReceivedEventArgs)
        {
            var context = exceptionReceivedEventArgs.ExceptionReceivedContext;
            var exception = exceptionReceivedEventArgs.Exception;
            _logger.LogError(exception, $"Message handler encountered an exception: {exception.Message}, Endpoint: {context.Endpoint}, Entity Path: {context.EntityPath}, Executing Action: {context.Action}");
            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public void Dispose()
        {
            _subscriptionClient?.CloseAsync().Wait();
            if (_eventHandler.EventRoutingMode == EventRoutingMode.Fanout)
            {
                _logger.LogInformation($"Deleting service bus subscription: {_settings.MsEventBusName} . {_topicName} . {_subscriptionName}");
                try
                {
                    _managementClient.DeleteSubscriptionIfExists(_topicName, _subscriptionName).Wait();
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error deleting service bus subscription: {_settings.MsEventBusName} . {_topicName} . {_subscriptionName}", ex);
                }
            }
            GC.SuppressFinalize(this);
        }
    }
}