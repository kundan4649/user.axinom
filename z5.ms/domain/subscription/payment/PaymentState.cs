namespace z5.ms.domain.subscription.payment
{
    /// <summary>State of a payment</summary>
    public enum PaymentState
    {
        /// <summary>Payment is completed. Default</summary>
        Completed = 1,

        /// <summary>A pending payment that has been used to activate / extend a subscription</summary>
        PendingAccepted = 2,

        /// <summary>A pending payment that has not been used to activate / extend a subscription. Pending payments will not be included in query results</summary>
        Pending = 3,
        
        /// <summary>A pending payment that has subsequently failed. Cancelled payments will not be included in query results</summary>
        Failed = 4,

        /// <summary>An undefined payment state. Take no action</summary>
        Unknown = 5,
    }
}