using Newtonsoft.Json;

namespace z5.ms.common.assets
{
    /// <summary>Shows the price of a <see cref="SubscriptionPlan"/> for a specific country and currency.</summary>
    public class PlanPrice
    {
        /// <summary>The country that this price and currency can be used in.</summary>
        [JsonProperty("country", Required = Required.Always)]
        public string Country { get; set; } 
        
        /// <summary>The price of a subscription plan.</summary>
        [JsonProperty("price", Required = Required.Always)]
        public double Price { get; set; }
        
        /// <summary>The currency, related to the price.</summary>
        [JsonProperty("currency", Required = Required.Always)]
        public string Currency { get; set; }
    }
}