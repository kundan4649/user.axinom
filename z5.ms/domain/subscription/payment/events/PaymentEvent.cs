using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using z5.ms.common.abstractions;
using z5.ms.domain.subscription.subscriptionplan;

namespace z5.ms.domain.subscription.payment.events
{
    /// <summary>An event describing an interaction with a payment provider</summary>
    public class PaymentEvent
    {
        /// <summary>Type of the payment event</summary>
        public PaymentEventType Type { get; set; }

        /// <summary>unique id of the subscription this payment is associated with</summary>
        public Guid? SubscriptionId { get; set; }

        /// <summary>unique id of the donation this payment is associated with</summary>
        public Guid? DonationId { get; set; }

        /// <summary>unique id of the purchase this payment is associated with</summary>
        public Guid? PurchaseId { get; set; }

        /// <summary>The subscription plan this payment is associated with</summary>
        public SubscriptionPlanEntity SubscriptionPlan { get; set; }

        /// <summary>Identifier for the subscription plan this playment is associated with</summary>
        [Obsolete("Full subscription plan should be provided")]
        public string SubscriptionPlanId { get; set; }

        /// <summary>An unique identifer from the payment provider identifying this payment</summary>
        public string TransactionIdentifier { get; set; }

        /// <summary>Identifies this subscription / purchase with the payment provider. Used for recurring billing and/or payment reconciliation</summary>
        public string ExternalIdentifier { get; set; }

        /// <summary>A human readable description of the payment </summary>
        public string PaymentDescription { get; set; }

        /// <summary>Is this a 0-value free trial payment?</summary>
        public int FreeTrial { get; set; }

        /// <summary>The amount of the payment</summary>
        public double? Amount { get; set; }

        /// <summary>The absolute amount that this subscription is discounted by</summary>
        public double? DiscountAmount { get; set; }

        /// <summary>Pending state of the payment</summary>
        public PaymentState PaymentState { get; set; } = PaymentState.Completed; // default supplied for backwards compatibility

        /// <summary>An error when processing the payment. If this field is included, the payment should NOT be considered as paid</summary>
        // TODO: Remove. handle errors inline
        public Error Error { get; set; }
    }

    /// <summary> Type of the payment event</summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum PaymentEventType
    {
        /// <summary>A subscription activation payment</summary>
        ActivateSubscription,

        /// <summary>A subscription renewal payment</summary>
        RenewSubscription,

        /// <summary>Subscription cancelled via the payment provider</summary>
        // TODO: Remove. handle cancellation inline
        CancelSubscription,

        /// <summary>A purchase activation payment</summary>
        ActivatePurchase,

        /// <summary>A donation activation payment</summary>
        ActivateDonation,
    }
}