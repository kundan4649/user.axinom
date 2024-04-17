using Amazon.SQS.Model;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace AwsClientLibrary
{
    public class SqsQueuePublisher<T> : IAWSPublisher<T>
    {
        private readonly ISqsQueueClient<T> _sqsQueueClient;
        public ILogger<SqsQueuePublisher<T>> Logger { get; }

        public SqsQueuePublisher(ISqsQueueClient<T> sqsQueueClient, ILogger<SqsQueuePublisher<T>> logger)
        {
            _sqsQueueClient = sqsQueueClient;
            Logger = logger;
        }

        public async Task Publish(T message)
        {

            var json = JsonConvert.SerializeObject(message);
            try
            {
                var sendMessageRequest = new SendMessageRequest
                {
                    QueueUrl = _sqsQueueClient.QueueUrl,
                    MessageBody = JsonConvert.SerializeObject(message)
                };

             var response = await this._sqsQueueClient.QueueServiceClient.SendMessageAsync(sendMessageRequest);
                Console.WriteLine($"{typeof(T).Name} published: {json}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to SendMessageAsync to SQS queue '{this._sqsQueueClient.QueueUrl}'. Exception: {ex.Message}");
                throw;
            }
        }
    }
}