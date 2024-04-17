using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;
using Amazon.SimpleNotificationService.Model;

namespace AwsClientLibrary
{
    public class SnsMessagePublisher<T> : IAWSPublisher<T>
    {
        private readonly ISnsTopicClient<T> _snsTopicClient;

        public ILogger<SnsMessagePublisher<T>> Logger { get; }

        public SnsMessagePublisher(ISnsTopicClient<T> snsTopicClient, ILogger<SnsMessagePublisher<T>> logger)
        {
            _snsTopicClient = snsTopicClient;
            Logger = logger;
        }

		public async Task Publish(T message)
        {
			try
			{
				var json = JsonConvert.SerializeObject(message);

				
				var publishRequest = new PublishRequest
				{
					TopicArn = _snsTopicClient.TopicArn,
					Message = JsonConvert.SerializeObject(message)
				};

				await this._snsTopicClient.NotificationServiceClient.PublishAsync(publishRequest).ConfigureAwait(false);
				this.Logger.LogDebug($"{typeof(T).Name} published: {json}");
			}
			catch (Exception ex)
			{
				this.Logger.LogError(ex, $"Failed to PublishAsync to SNS Topic '{this._snsTopicClient.TopicArn}'. Exception: {ex.Message}");
				throw;
			}
		}

		/// <inheritdoc />
		public void Dispose()
		{
			this._snsTopicClient.NotificationServiceClient.Dispose();
			GC.SuppressFinalize(this);
		}
	}
}
