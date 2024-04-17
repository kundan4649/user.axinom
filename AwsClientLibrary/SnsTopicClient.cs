using Amazon.SimpleNotificationService;

namespace AwsClientLibrary
{
    public class SnsTopicClient<T> : ISnsTopicClient<T>
    {
        private readonly IConfigurationService _configurationService;

        public AmazonSimpleNotificationServiceClient NotificationServiceClient { get; set; }

        public string TopicArn { get; set; }

        public SnsTopicClient(IConfigurationService configurationService)
        {
            _configurationService = configurationService;

            this.NotificationServiceClient = CreateClient();
        }
        public AmazonSimpleNotificationServiceClient CreateClient()
        {
            this.TopicArn = _configurationService.GetConfiguration()[typeof(T).Name + "_SnsTopicArn"];

            var snsConfig = new AmazonSimpleNotificationServiceConfig
            {
                //TODO: read url from appsetting.json
                // "http://sns.ap-south-1.amazonaws.com"
                ServiceURL = _configurationService.GetConfiguration()["TopicServiceUrl"]
            };

            return new AmazonSimpleNotificationServiceClient(snsConfig);
        }
    }
}