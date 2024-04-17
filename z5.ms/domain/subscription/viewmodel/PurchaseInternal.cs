using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

namespace z5.ms.domain.subscription.viewmodel
{
    /// <summary>A purchase response for the internal api</summary>
    public class PurchaseInternal
    {
        /// <summary>The unique ID of the purchase.</summary>
        [JsonProperty("id", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public Guid Id { get; set; }

        //TODO: Remove once no longer required
        /// <summary>The unique ID of the customer of this subscription.</summary>
        [JsonProperty("customer_id", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public Guid CustomerId { get; set; }

        /// <summary>The customer that owns this purchase.</summary>
        [JsonProperty("customer", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public Subscriber Subscriber { get; set; }

        /// <summary>The subscroption plan id.</summary>
        [JsonProperty("subscription_plan_id", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string SubscriptionPlanId { get; set; }

        /// <summary>The date at which point the purchase ends. Null for infinite length.</summary>
        [JsonProperty("purchase_end", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? PurchaseEnd { get; set; }

        /// <summary>State of the subscription</summary>
        [JsonProperty("state", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(StringEnumConverter))]
        public PurchaseState State { get; set; }

        /// <summary>The asset that this purchase relates to.</summary>
        [JsonProperty("asset_id", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string AssetId { get; set; }

        /// <summary>The type of the payment provider. This cannot be changed once it is set.</summary>
        [JsonProperty("payment_provider", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string PaymentProviderName { get; set; }

        /// <summary>The date at which point the purchase was created.</summary>
        [JsonProperty("date", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
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

    /// <summary>
    /// Sorting options for listing purchases
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum PurchaseInternalSortByField
    {
        /// <summary>Sort by date</summary>
        [EnumMember(Value = "date")]
        Date,

        /// <summary>Sort by purchase end date</summary>
        [EnumMember(Value = "purchase_end")]
        PurchaseEnd,

        /// <summary>Sort by subscription plan id</summary>
        [EnumMember(Value = "subscription_plan_id")]
        SubscriptionPlanId,

        /// <summary>Sort by subscription plan id</summary>
        [EnumMember(Value = "asset_id")]
        AssetId
    }
}
