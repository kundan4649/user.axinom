using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.InteropExtensions;
using Microsoft.Azure.ServiceBus.Management;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using z5.ms.common.abstractions;
using z5.ms.common.extensions;
using z5.ms.common.infrastructure.assetcache.config;

namespace z5.ms.common.infrastructure.assetcache
{
    /// <summary>CMS publishing integration service</summary>
    public interface ICmsPublishService : IDisposable
    {
        /// <summary>Initialize the service, create necessary resources etc.</summary>
        Task CreateSubscription(string subscriptionName);

        /// <summary>Initialize the service, create necessary resources etc.</summary>
        void StartProcessingMessages();

        /// <summary>Provide a list of asset types to be loaded. Any not listed will be ignored</summary>
        void FilterAssetTypes(params int[] filterAssetTypes);
    }

    /// <inheritdoc cref="ICmsPublishService" />
    /// <remarks>
    /// Currently uses the ReceiveMode.ReceiveAndDelete mode, which means that the message is deleted immediately after
    /// it is read, even if there is an error afterwards, during message processing..
    /// Seems like an OK solution as we only expect publish messages on this topic and there is no need to keep messages
    /// that are corrupted/in other format.
    /// </remarks>
    public class CmsPublishService : ICmsPublishService
    {
        private readonly IAssetsCache _cache;
        private readonly ManagementClient _managementClient;
        private readonly ILogger _logger;
        private ISubscriptionClient _subscriptionClient;
        private readonly SyncOptions _settings;
        private IEnumerable<int> _filterAssetTypes;
        private string _subscriptionName;

        /// <inheritdoc />
        public CmsPublishService(IAssetsCache cache, IOptions<SyncOptions> options, ILoggerFactory loggerFactory)
        {
            _cache = cache;
            _settings = options.Value;
            _managementClient = new ManagementClient(_settings.CmStoMsConnection);
            _logger = loggerFactory.CreateLogger("PublishService");
        }

        /// <inheritdoc />
        public void FilterAssetTypes(params int[] filterAssetTypes)
        {
            _filterAssetTypes = filterAssetTypes;
        }

        /// <inheritdoc />
        public async Task CreateSubscription(string subscriptionName)
        {
            _subscriptionName = subscriptionName;
            _logger.LogInformation($"Recreating service bus subscription: {_settings.PublishBusName} . {_settings.PublishTopicName} . {_subscriptionName}");
            await _managementClient.DeleteSubscriptionIfExists(_settings.PublishTopicName, subscriptionName);
            await _managementClient.CreateSubscriptionIfNotExists(new SubscriptionDescription(_settings.PublishTopicName, subscriptionName));
        }

        /// <inheritdoc />
        public void StartProcessingMessages()
        {
            _subscriptionClient = new SubscriptionClient(_settings.CmStoMsConnection, _settings.PublishTopicName,
                _subscriptionName, ReceiveMode.ReceiveAndDelete);

            var messageHandlerOptions = new MessageHandlerOptions(ExceptionReceivedHandler)
            {
                MaxConcurrentCalls = 1,
                AutoComplete = true
            };

            _subscriptionClient.RegisterMessageHandler(ProcessMessagesAsync, messageHandlerOptions);

            _logger.LogInformation("PublishService initialized");
        }

        // TODO: how to handle referenced assets when deleting an asset
        /// <summary>Process an incoming publish message</summary>
        /// <remarks>Upserts or removes a cache entry</remarks>
        private async Task ProcessMessagesAsync(Message message, CancellationToken token)
        {
            var json = message.GetBody<string>();
            var publishMessage = JsonConvert.DeserializeObject<PublishMessage>(json);
            var assetType = publishMessage.AssetId.GetDirectoryAssetType();

            if (_filterAssetTypes != null && !_filterAssetTypes.Contains(assetType))
                return;

            switch (publishMessage.Type)
            {
                case PublishMessageOperationType.Replace:
                    await _cache.AddAsset(publishMessage.AssetId, publishMessage.AssetId, assetType);
                    break;
                case PublishMessageOperationType.PostCompare:
                    _cache.RemoveAsset(publishMessage.AssetId);
                    break;
                default:
                    _logger.LogDebug($"No action defined for OperationType {publishMessage.Type}");
                    break;
            }
        }

        /// <summary>Use this Handler to look at the exceptions received on the MessagePump</summary>
        private Task ExceptionReceivedHandler(ExceptionReceivedEventArgs exceptionReceivedEventArgs)
        {
            var context = exceptionReceivedEventArgs.ExceptionReceivedContext;
            var exception = exceptionReceivedEventArgs.Exception;
            _logger.LogError(exception, $"Message handler encountered an exception: {exception.Message}, Endpoint: {context.Endpoint}, Entity Path: {context.EntityPath}, Executing Action: {context.Action}");
            return Task.CompletedTask;
        }

        private void ReleaseUnmanagedResources()
        {
            _subscriptionClient?.CloseAsync().Wait();
            _logger.LogInformation($"Deleting service bus subscription: {_settings.PublishBusName} . {_settings.PublishTopicName} . {_subscriptionName}");
            _managementClient.DeleteSubscriptionIfExists(_settings.PublishTopicName, _subscriptionName).Wait();
        }

        /// <inheritdoc />
        public void Dispose()
        {
            ReleaseUnmanagedResources();
            GC.SuppressFinalize(this);
        }

        /// <inheritdoc />
        ~CmsPublishService()
        {
            ReleaseUnmanagedResources();
        }
    }
}