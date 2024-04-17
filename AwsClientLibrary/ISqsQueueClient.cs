using Amazon.SQS;

namespace AwsClientLibrary
{
    public interface ISqsQueueClient<T>
    {
        AmazonSQSClient QueueServiceClient { get; set; }
        string QueueUrl { get; set; }
    }
}