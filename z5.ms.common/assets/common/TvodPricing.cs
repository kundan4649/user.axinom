using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace z5.ms.common.assets.common
{
    /// <summary>Interface for a response model implementing the TvodPricing property</summary>
    public interface ITvodPricing
    {
        /// <summary>TvodPricing property mapped from the cheapest subscription plan with matching tvod tier for a given country</summary>
        TvodPricing TvodPricing { get; set; }
    }

    /// <summary>Tvod pricing information</summary>
    public class TvodPricing
    {
        /// <summary>A price tier at which the asset should be available in that country</summary>
        [JsonProperty("tier", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string TvodTier { get; set; }

        /// <summary>The amount of money that a subscription costs for the duration of the billing frequency. This includes taxes.</summary>
        [JsonProperty("price", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        [Range(0.0, 9999999.0)]
        public double? Price { get; set; }

        /// <summary>The currency that should be used.</summary>
        [JsonProperty("currency", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        [StringLength(int.MaxValue, MinimumLength = 1)]
        public string Currency { get; set; }

        /// <summary>The duration type how long a purchase is valid</summary>
        [JsonProperty("billing_cycle_type", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string BillingCycleType { get; set; }

        /// <summary>The duration in "billing_cycle_type" how long a purchase can be used before it must be paid again.</summary>
        [JsonProperty("billing_frequency", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        [Range(1.0, double.MaxValue)]
        public int? BillingFrequency { get; set; }
    }
}