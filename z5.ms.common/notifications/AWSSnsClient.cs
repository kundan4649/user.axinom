using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace z5.ms.common.notifications
{
    /// <summary>A client for pushing data to SNS asyncronously</summary>
    public interface IAWSSnsClient
    {
        /// <summary>Handles a sns request</summary>
        /// <param name="Simple Notification Service"></param>
        Task<PublishResponse> SendToSNS(Dictionary<string, string> requestParameters);
    }

    public class AWSSnsClient : IAWSSnsClient
    {
        private readonly ILogger<AWSNotificationQueue> _logger;
        private readonly IAmazonSimpleNotificationService _snsClient;
        private string TopicArn { get; set; }
        /// <inheritdoc/>
        public AWSSnsClient(IOptions<NotificationOptions> options, ILogger<AWSNotificationQueue> logger, IAmazonSimpleNotificationService snsClient)
        {
            _logger = logger;           
            _snsClient = snsClient;
            InitializeQueue("registration_incentive").Wait();
        }

        /// <inheritdoc />
        private async Task InitializeQueue(string snsName)
        {
            try
            {
                var responseResult = await _snsClient.FindTopicAsync(snsName);
                if (responseResult != null)
                    this.TopicArn = responseResult.TopicArn;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get TopicARN for {snsName}. Error: {ex.Message}", ex);
            }
        }

        public async Task<PublishResponse> SendToSNS(Dictionary<string, string> requestParameters)
        {

            try
            {
                var json = JsonConvert.SerializeObject(requestParameters);

                var publishRequest = new PublishRequest
                {
                    TopicArn = this.TopicArn,
                    Message = JsonConvert.SerializeObject(requestParameters)
                };
                var result = await _snsClient.PublishAsync(publishRequest).ConfigureAwait(false);
                if (result != null && result.HttpStatusCode == System.Net.HttpStatusCode.OK)
                    return result;
                
                throw new Exception(result.HttpStatusCode.ToString());
              
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to PublishAsync to SNS Topic '{this.TopicArn}'. Exception: {ex.Message}");
                throw;
            }
        }
    }
}
