using System.Collections.Generic;

namespace z5.ms.domain.subscription.payment.paymentproviders
{
    /// <summary>
    /// Configuration options for Dialog payment service
    /// </summary>
    public class DialogOptions
    {
        /// <summary>Allowed ip range for promotional subscriptions via Dialog</summary>
        public List<string> DialogPromotionAllowedIps { get; set; }
    }
}
