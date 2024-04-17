using System;
using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Queue;
using Newtonsoft.Json;
using z5.ms.common.extensions;
using Amazon.SQS.Model;
using Amazon.SQS;
using System.Threading.Tasks;
using Amazon;
using System.Net;

namespace z5.ms.common.notifications
{
    /// <summary>A client for sending notifications asyncronously</summary>
    public interface IAWSNotificationQueue
    {
        /// <summary>Send a notification</summary>
        /// <param name="notification"></param>
        void Send(Notification notification);
    }

    /// <summary>AWS Notification queue implementation using an AWS storage queue </summary>
    public class AWSNotificationQueue : IAWSNotificationQueue
    {
        private readonly ILogger<AWSNotificationQueue> _logger;
        private readonly IAmazonSQS _sqsClient;
        private string _awsQueueUrl;
        /// <inheritdoc/>
        public AWSNotificationQueue(IOptions<NotificationOptions> options, ILogger<AWSNotificationQueue> logger, IAmazonSQS sqsClient)
        {
            _logger = logger;           
            _sqsClient = sqsClient;
            InitializeQueue("notifications").Wait();
            
        }

        /// <inheritdoc />
        public async void Send(Notification notification)
        {
            try
            {
                await SendtoAwsQueue(notification);
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed to send notification {notification.Type.EnumMemberValue().ToLower()} to {notification.To}", e);
            }
        }

        private async Task InitializeQueue(string queueName)
        {
            try
            {
                var responseResult = await _sqsClient.GetQueueUrlAsync(queueName);
                if (responseResult.HttpStatusCode == HttpStatusCode.OK)
                    _awsQueueUrl = responseResult.QueueUrl;

            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get QueueUrl for {queueName}. Error: {ex.Message}", ex);
            }

            if (string.IsNullOrEmpty(_awsQueueUrl))
            {
                var createResponse = await _sqsClient.CreateQueueAsync(new CreateQueueRequest { QueueName = queueName });
                if (createResponse.HttpStatusCode == HttpStatusCode.OK)
                    _awsQueueUrl = createResponse.QueueUrl;
            }
        }

        public async Task<SendMessageResponse> SendtoAwsQueue(Notification notification)
        {
            var msgString = JsonConvert.SerializeObject(notification);
            int delaySeconds = 0;
            var request = new SendMessageRequest
            {
                QueueUrl = _awsQueueUrl,
                MessageBody = msgString,
                DelaySeconds = (int)TimeSpan.FromSeconds(delaySeconds).TotalSeconds
            };

            return await _sqsClient.SendMessageAsync(request);
        }
    }
}
