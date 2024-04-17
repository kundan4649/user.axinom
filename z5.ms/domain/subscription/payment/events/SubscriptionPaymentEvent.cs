using System;

namespace z5.ms.domain.subscription.payment.events
{
    /// <summary>An event describing a payment for a subscription</summary>
    public class SubscriptionPaymentEvent : PaymentEvent
    {
        /// <summary>Start date for the subscription. Not required; depends on payment provider</summary>
        public DateTime? SubscriptionStart { get; set; }

        /// <summary>End date for the subscription. Not required; depends on payment provider</summary>
        public DateTime? SubscriptionEnd { get; set; }

        /// <summary>Indicates subscription of current payment is a recurring subscription or not. If provided it should override subscription plan</summary>
        public bool? Recurring { get; set; }
    }
}