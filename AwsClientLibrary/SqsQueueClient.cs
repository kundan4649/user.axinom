using System;
using System.Threading.Tasks;
using Amazon.SQS;
using Amazon.SQS.Model;

namespace AwsClientLibrary
{
    public class SqsQueueClient<T> : ISqsQueueClient<T>
    {
        private readonly IConfigurationService _configurationService;
        public AmazonSQSClient QueueServiceClient { get; set; }

        public string QueueUrl { get; set; }

        public SqsQueueClient(IConfigurationService configurationService)
        {
            this._configurationService = configurationService;
            CreateClient().Wait();
        }

        public async Task CreateClient()
        {
            var queueName = this._configurationService.GetConfiguration()[typeof(T).Name + "_SqsQueueName"];
            var sqsConfig = new AmazonSQSConfig
            {
                //http://sqs.ap-south-1.amazonaws.com
                ServiceURL = this._configurationService.GetConfiguration()["QueueServiceUrl"]
            };

            QueueServiceClient = new AmazonSQSClient(sqsConfig);

            try
            {
                this.QueueUrl = await GetQueueUrl(queueName).ConfigureAwait(false);
            }
            catch (QueueDoesNotExistException ex)
            {
                throw new Exception($"Could not retrieve the URL for the queue '{queueName}' as it does not exist or you do not have access to it.", ex);
            }
        }

        private async Task<string> GetQueueUrl(string queueName)
        {
            try
            {
                var response = await this.QueueServiceClient.GetQueueUrlAsync(queueName);
                return response.QueueUrl;
            }
            catch (QueueDoesNotExistException ex)
            {
                throw new Exception($"Could not retrieve the URL for the queue '{queueName}' as it does not exist or you do not have access to it.", ex);
            }
        }

    }
}