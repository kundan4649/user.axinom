using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace z5.ms.domain.subscription.viewmodel
{
    /// <summary>A donation response for the internal api</summary>
    public class DonationInternal
    {
        /// <summary>The unique ID of the donation</summary>
        [JsonProperty("id", Required = Required.Always)]
        [Required]
        public Guid Id { get; set; }

        //TODO: Remove once no longer required
        /// <summary>The unique ID of the customer of this subscription.</summary>
        [JsonProperty("customer_id", Required = Required.Always)]
        public Guid CustomerId { get; set; }
        
        /// <summary>The customer that made this donation.</summary>
        [JsonProperty("customer", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public Subscriber Subscriber { get; set; }

        /// <summary>The unique ID of the subscription plan which was used to make this donation.</summary>
        [JsonProperty("subscription_plan_id", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string SubscriptionPlanId { get; set; }

        /// <summary>The amount of money that was paid</summary>
        [JsonProperty("amount", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public double Amount { get; set; }

        /// <summary>The currency in which was paid.</summary>
        [JsonProperty("currency", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        [StringLength(20, MinimumLength = 1)]
        public string Currency { get; set; }

        /// <summary>The type of the payment provider. This cannot be changed once it is set.</summary>
        [JsonProperty("payment_provider", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string PaymentProviderName { get; set; }

        /// <summary>The date at which point the donation was created.</summary>
        [JsonProperty("date", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public DateTime Date { get; set; }
    }
}
