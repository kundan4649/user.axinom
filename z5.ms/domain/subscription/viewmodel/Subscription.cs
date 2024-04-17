using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using z5.ms.common.assets;
using z5.ms.common.helpers;

namespace z5.ms.domain.subscription.viewmodel
{
    /// <summary>
    /// Subscription details response model for external APIs
    /// </summary>
    public class Subscription
    {
        /// <summary>The unique ID of the subscription. This is available for non-trial ones.</summary>
        [JsonProperty("id", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public Guid? Id { get; set; }

        /// <summary>The unique ID of the user of this subscription.</summary>
        [JsonProperty("user_id", Required = Required.Always)]
        [Required]
        public Guid UserId { get; set; }

        /// <summary>The external identifier like a subscription ID or similar of the related subscription in the payment providers system</summary>
        [JsonProperty("identifier", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        [StringLength(200, MinimumLength = 1)]
        public string Identifier { get; set; }

        /// <summary>Subscription plan associated with the subscription</summary>
        [JsonProperty("subscription_plan", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public SubscriptionPlan SubscriptionPlan { get; set; }

        /// <summary>The date at which point the subscription started.</summary>
        [JsonProperty("subscription_start", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? SubscriptionStart { get; set; }

        /// <summary>The date at which point the subscription ends.</summary>
        [JsonProperty("subscription_end", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(LastSecondOfDayTimeConverter))]
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
        [JsonProperty("free_trial", Required = Required.Default)]
        public int? FreeTrial { get; set; }

        /// <summary>The creation date of subscription</summary>
        [JsonProperty("create_date", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? Created { get; set; }

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
        
        /// <summary>The promo code used for this subscription.</summary>
        [JsonProperty("promo_code", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string PromoCode { get; set; }
        
        /// <summary>The maximum number of billing cycles that the promo code can be applied for.</summary>
        [JsonProperty("allowed_billing_cycles", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public int AllowedBillingCycles { get; set; }
        
        /// <summary>The amount of billing cycled that have already been billed using the discount from the promotion.</summary>
        [JsonProperty("used_billing_cycles", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public int UsedBillingCycles { get; set; }
    }
}
