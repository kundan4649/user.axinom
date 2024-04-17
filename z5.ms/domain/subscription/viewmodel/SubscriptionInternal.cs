using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using z5.ms.common.helpers;

namespace z5.ms.domain.subscription.viewmodel
{
    /// <summary>A subscription response for the internal api</summary>
    public class SubscriptionInternal
    {
        /// <summary>The unique ID of the subscription. This is available for non-trial ones.</summary>
        [JsonProperty("id", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public Guid? Id { get; set; }

        //TODO: Remove once no longer required
        /// <summary>The unique ID of the customer of this subscription.</summary>
        [JsonProperty("customer_id", Required = Required.Always)]
        public Guid CustomerId { get; set; }
        
        /// <summary>The customer that owns this subscription.</summary>
        [JsonProperty("customer", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        [Required]
        public Subscriber Subscriber { get; set; }

        /// <summary>The external identifier like a subscription ID or similar of the related subscription in the payment providers system</summary>
        [JsonProperty("identifier", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        [StringLength(200, MinimumLength = 1)]
        public string Identifier { get; set; }

        /// <summary>Subscription plan associated with the subscription</summary>
        [JsonProperty("subscription_plan_id", Required = Required.Always)]
        public string SubscriptionPlanId { get; set; }

        /// <summary>The date at which point the subscription created.</summary>
        [JsonProperty("created_date", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? Created { get; set; }

        /// <summary>The date at which point the subscription started.</summary>
        [JsonProperty("subscription_start", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? SubscriptionStart { get; set; }

        /// <summary>The date at which point the subscription ends.</summary>
        [JsonProperty("subscription_end", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(DateNotTimeConverter))]
        public DateTime? SubscriptionEnd { get; set; }

        /// <summary>State of the subscription</summary>
        [JsonProperty("state", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(StringEnumConverter))]
        public SubscriptionState State { get; set; }

        /// <summary>If the subscription end date is reached - should the next payment be conducted or not.</summary>
        [JsonProperty("recurring_enabled", Required = Required.Always)]
        public bool RecurringEnabled { get; set; }

        /// <summary>The type of the payment provider. This cannot be changed once it is set.</summary>
        [JsonProperty("payment_provider", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string PaymentProviderName { get; set; }

        /// <summary>The number of days before the first payment will be charged.</summary>
        [JsonProperty("free_trial", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public int? FreeTrial { get; set; }

        /// <summary>Success and error notes about subscription history.</summary>
        [JsonProperty("notes", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string Notes { get; set; }

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

        /// <summary>The absolute amount that this subscription is discounted by</summary>
        [JsonProperty("discount_amount", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public double? DiscountAmount { get; set; }
    }

    /// <summary>
    /// Sorting options for listing subscriptions
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum SubscriptionInternalSortByField
    {
        /// <summary>Sort by subscription start date</summary>
        [EnumMember(Value = "subscription_start")]
        SubscriptionStart,

        /// <summary>Sort by subscription end date</summary>
        [EnumMember(Value = "subscription_end")]
        SubscriptionEnd,

        /// <summary>Sort by recurring enabled</summary>
        [EnumMember(Value = "recurring_enabled")]
        RecurringEnabled,

        /// <summary>Sort by free trial</summary>
        [EnumMember(Value = "free_trial")]
        FreeTrial,

        /// <summary>Sort by subscription plan id</summary>
        [EnumMember(Value = "subscription_plan_id")]
        SubscriptionPlanId
    }
}
