using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace z5.ms.domain.subscription.viewmodel
{
    public class InternalSubscription2
    {
        [JsonProperty("id", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string Id { get; set; }
        [JsonProperty("user_id", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public Guid UserId { get; set; }
        [JsonProperty("subscription_plan", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public SubscriptionPlan SubscriptionPlan { get; set; }
        [JsonProperty("subscription_end", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public DateTime SubscriptionEnd { get; set; }
        [JsonProperty("state", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string State { get; set; }
    }

    public class SubscriptionPlan
    {
        [JsonProperty("subscription_end", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string Id { get; set; }
        [JsonProperty("price", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public decimal Price { get; set; }
        [JsonProperty("start", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public DateTime Start { get; set; }
        [JsonProperty("end", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public DateTime End { get; set; }
        [JsonProperty("number_of_supported_devices", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string NumberOfSupportedDevices { get; set; }
    }
}