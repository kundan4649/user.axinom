using Newtonsoft.Json;

namespace z5.ms.domain.subscription.viewmodel
{
    /// <summary>Response model for adyen notifications</summary>
    public class AdyenNotificationResponse
    {
        /// <inheritdoc />
        public AdyenNotificationResponse(string response)
        {
            NotificationResponse = response;
        }

        /// <summary>Notification response string ("[accepted]" for success)</summary>
        [JsonProperty(PropertyName = "notificationResponse")]
        public string NotificationResponse { get; }
    }
}
