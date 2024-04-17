using Newtonsoft.Json;

namespace z5.ms.domain.subscription.viewmodel
{
    /// <summary>
    /// Request model to create a promotional subscription
    /// </summary>
    public class CreatePromoSubscription
    {
        /// <summary>A token that could provide prove that a user is eligable for a promotional subscription.</summary>
        [JsonProperty("token", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string Token { get; set; }
    }
}
