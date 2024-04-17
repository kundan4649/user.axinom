using System;
using Newtonsoft.Json;

namespace z5.ms.domain.subscription.viewmodel
{
    /// <summary>Active subscription information from in-app purchases</summary>
    public class ActiveSubscription
    {
        /// <summary>The ID or name of the product.</summary>
        [JsonProperty("product", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string Product { get; set; }

        /// <summary>If the subscription is active today</summary>
        [JsonProperty("is_active", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public bool IsActive { get; set; }

        /// <summary>If the subscription is still enabled for recurring (true) or cancelled (false)</summary>
        [JsonProperty("recurring_enabled", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public bool? RecurringEnabled { get; set; }

        /// <summary>The date at which point the subscription ends.</summary>
        [JsonProperty("subscription_end", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? SubscriptionEnd { get; set; }
    }
}
