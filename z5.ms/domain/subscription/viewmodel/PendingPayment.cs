using System;

namespace z5.ms.domain.subscription.viewmodel
{
    /// <summary>Azure queue message type to handle pending status payments</summary>
    public class PendingPayment
    {
        /// <summary>PayTm payment order id</summary>
        public string OrderId { get; set; }

        /// <summary>Status check retry count</summary>
        public int RetryCount { get; set; }

        /// <summary>Time of the last retry attempt</summary>
        public DateTime LastRetry { get; set; }
    }
}
