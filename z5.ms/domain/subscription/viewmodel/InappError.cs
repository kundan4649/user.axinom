using System;
using Newtonsoft.Json;

namespace z5.ms.domain.subscription.viewmodel
{
    /// <summary>In-App purchases callback error request model</summary>
    public class InappSubscriptionError
    {
        /// <summary>This is the unique identifier for the prepared subscription.</summary>
        [JsonProperty("subscription_id", Required = Required.Always)]
        public Guid SubscriptionId { get; set; }

        /// <summary>The error code/enum coming from the Google/Apple API</summary>
        [JsonProperty("code", Required = Required.Always)]
        public string Code { get; set; }

        /// <summary>The error message coming from the Google/Apple API</summary>
        [JsonProperty("message", Required = Required.Always)]
        public string Message { get; set; }
    }

    /// <summary>In-App purchases callback error request model</summary>
    public class InappPurchaseError
    {
        /// <summary>This is the unique identifier for the prepared purchase.</summary>
        [JsonProperty("purchase_id", Required = Required.Always)]
        public Guid PurchaseId { get; set; }

        /// <summary>The error code/enum coming from the Google/Apple API</summary>
        [JsonProperty("code", Required = Required.Always)]
        public string Code { get; set; }

        /// <summary>The error message coming from the Google/Apple API</summary>
        [JsonProperty("message", Required = Required.Always)]
        public string Message { get; set; }
    }
}
