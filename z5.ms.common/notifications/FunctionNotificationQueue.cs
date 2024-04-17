using System;
using Microsoft.Extensions.Logging;
using z5.ms.common.abstractions;
using z5.ms.common.extensions;

namespace z5.ms.common.notifications
{
    /// <summary>Notification queue to be used in functions </summary>
    public class FunctionNotificationQueue : INotificationQueue
    {
        private readonly IPublisher<Notification> _queue;
        private readonly ILogger _logger;

        /// <inheritdoc />
        public FunctionNotificationQueue(IPublisher<Notification> queue, ILogger logger)
        {
            _queue = queue;
            _logger = logger;
        }

        /// <inheritdoc />
        public void Send(Notification notification)
        {
            try
            {
                _queue.Publish(notification);

            }
            catch (Exception e)
            {
                _logger.LogError($"Failed to send notification {notification.Type.EnumMemberValue().ToLower()} to {notification.To}", e);
            }
        }
    }
}
