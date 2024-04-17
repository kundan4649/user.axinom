using Amazon.SimpleNotificationService;

namespace AwsClientLibrary
{
    public interface ISnsTopicClient<T>
    {
        AmazonSimpleNotificationServiceClient NotificationServiceClient { get; set; }
        string TopicArn { get; set; }
    }
}