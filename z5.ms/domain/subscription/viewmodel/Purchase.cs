using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using z5.ms.common.assets;

namespace z5.ms.domain.subscription.viewmodel
{
    /// <summary>
    /// Response model for a purchase
    /// </summary>
    public class Purchase
    {
        /// <summary>The unique ID of the purchase</summary>
        [JsonProperty("id", Required = Required.Always)]
        [Required]
        public Guid Id { get; set; }

        /// <summary>The unique ID of the user who owns these purchase.</summary>
        [JsonProperty("user_id", Required = Required.Always)]
        [Required]
        public Guid UserId { get; set; }

        /// <summary>The unique ID of the subscription plan which was used to make this purchase</summary>
        [JsonProperty("subscription_plan", Required = Required.Always)]
        [Required]
        public SubscriptionPlan SubscriptionPlan { get; set; }

        /// <summary>The date at which point the purchase ends. Null for infinite length.</summary>
        [JsonProperty("purchase_end", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? PurchaseEnd { get; set; }

        /// <summary>State of the subscription</summary>
        [JsonProperty("state", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(StringEnumConverter))]
        public PurchaseState State { get; set; }

        /// <summary>The unique ID of the asset which was purchased or rented.</summary>
        [JsonProperty("asset_id", Required = Required.Always)]
        [Required]
        public string AssetId { get; set; }

        /// <summary>The type of the payment provider. This cannot be changed once it is set.</summary>
        [JsonProperty("payment_provider", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string PaymentProviderName { get; set; }

        /// <summary>The date at which point the purchase was created.</summary>
        [JsonProperty("date", Required = Required.Always)]
        public DateTime Date { get; set; }

        /// <summary>IP address of the user</summary>
        [JsonProperty("ip_address", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string IpAddress { get; set; }

        /// <summary>Country in “ISO 3166-1 alpha-2” format from where the user initially registered.</summary>
        [JsonProperty("country", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string RegistrationCountry { get; set; }

        /// <summary>Registration region of the user from Maxmind DB</summary>
        [JsonProperty("region", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string RegistrationRegion { get; set; }

        /// <summary>Additional information for user to be stored in DB for reporting purposes</summary>
        [JsonProperty("additional", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public JObject Additional { get; set; }
    }

}
