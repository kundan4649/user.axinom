using System;

namespace z5.ms.domain.subscription.payment.paymentproviders
{
    /// <summary>Result model for subscription, purchase or donation activation</summary>
    public class ActivateResult
    {
        /// <summary>User id of the activated subscription, purchase or donation</summary>
        public Guid UserId { get; set; }

        /// <summary>Subscription plan id of the activated subscription, purchase or donation</summary>
        public string SubscriptionPlanId { get; set; }

        /// <summary>Asset id of the purchased item (used for purchase only)</summary>
        public string AssetId { get; set; }
    }
}
