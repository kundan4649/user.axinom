using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace z5.ms.domain.subscription.viewmodel
{
    /// <summary>
    /// Payment details response model
    /// </summary>
    public class Payment
    {
        /// <summary>The unique ID of the payment</summary>
        [JsonProperty("id", Required = Required.Always)]
        [Required]
        public Guid Id { get; set; }

        /// <summary>The unique ID of the subscription which owns this payment (alt. purchase_id / donation_id).</summary>
        [JsonProperty("subscription_id", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public Guid? SubscriptionId { get; set; }

        /// <summary>The unique ID of the purchase which owns this payment (alt. subscription_id / donation_id).</summary>
        [JsonProperty("purchase_id", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public Guid? PurchaseId { get; set; }

        /// <summary>The unique ID of the donation which owns this payment (alt. subscription_id /purchase_id).</summary>
        [JsonProperty("donation_id", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public Guid? DonationId { get; set; }

        /// <summary>The amount of money that was paid</summary>
        [JsonProperty("amount", Required = Required.Always)]
        [Required]
        public double Amount { get; set; }

        /// <summary>The absolute amount that this subscription is discounted by</summary>
        [JsonProperty("discount_amount", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public double? DiscountAmount { get; set; }

        /// <summary>The currency in which was paid.</summary>
        [JsonProperty("currency", Required = Required.Always)]
        [Required]
        [StringLength(20, MinimumLength = 1)]
        public string Currency { get; set; }

        /// <summary>List of taxes applied to a payment</summary>
        [JsonProperty("taxes", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public List<Tax> Taxes { get; set; }

        /// <summary>The external ID like a transaction ID or similar of the related payment in the payment providers system</summary>
        [JsonProperty("identifier", Required = Required.Always)]
        [Required]
        [StringLength(200, MinimumLength = 1)]
        public string Identifier { get; set; }

        /// <summary>Additional details for the used payment provider e.g. card brand for Gateway billing or mobile carrier name for Carrier billing</summary>
        [JsonProperty("description", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string Description { get; set; }

        /// <summary>The type of the payment provider. This cannot be changed once it is set.</summary>
        [JsonProperty("payment_provider", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string PaymentProvider { get; set; }

        /// <summary>Date Time of the payment</summary>
        [JsonProperty("date", Required = Required.Always)]
        [Required]
        public DateTime Date { get; set; }
    }
}
