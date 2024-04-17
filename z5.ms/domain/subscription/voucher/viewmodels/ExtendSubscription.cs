using Microsoft.AspNetCore.Mvc;

namespace z5.ms.domain.subscription.voucher.viewmodels
{
    /// <summary> Subscription extension request model </summary>
    public class ExtendSubscription
    {
        /// <summary>Voucher code to be used to extend subscription</summary>
        [FromQuery(Name = "voucher_code")]
        public string VoucherCode { get; set; }

        /// <summary>Subscription plan to be used with voucher</summary>
        [FromQuery(Name = "subscription_plan_id")]
        public string SubscriptionPlanId { get; set; }
    }
}

