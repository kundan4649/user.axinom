using System;
using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Queue;
using Newtonsoft.Json;
using z5.ms.common.extensions;

namespace z5.ms.common.notifications
{
    /// <summary>A client for sending notifications asyncronously</summary>
    public interface INotificationQueue
    {
        /// <summary>Send a notification</summary>
        /// <param name="notification"></param>
        void Send(Notification notification);
    }

    /// <summary>Notification queue implementation using an Azure storage queue </summary>
    public class NotificationQueue : INotificationQueue
    {
        private readonly ILogger<NotificationQueue> _logger;
        private readonly CloudQueue _queue;

        /// <inheritdoc/>
        public NotificationQueue(IOptions<NotificationOptions> options, ILogger<NotificationQueue> logger)
        {
            _queue = InitializeQueue("notifications", options.Value.NotificationQueueConnection);
            _logger = logger;
        }

        /// <inheritdoc />
        public async void Send(Notification notification)
        {
            try
            {
                var msgString = JsonConvert.SerializeObject(notification);
                await _queue.AddMessageAsync(new CloudQueueMessage(msgString));
            }
            catch (Exception e)
            {
                 _logger.LogError($"Failed to send notification {notification.Type.EnumMemberValue().ToLower()} to {notification.To}", e);
            }
        }

        private CloudQueue InitializeQueue(string queueName, string queueStorageConnection)
        {
            var queue = CloudStorageAccount.Parse(queueStorageConnection)
                .CreateCloudQueueClient()
                .GetQueueReference(queueName);

            queue.CreateIfNotExistsAsync().Wait();
            return queue;
        }
    }
}
