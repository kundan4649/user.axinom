using System.Runtime.Serialization;

namespace z5.ms.domain.subscription.viewmodel
{
    /// <summary>
    /// State options for subscriptions
    /// </summary>
    public enum SubscriptionState
    {
        /// <summary>Subscription waiting for payment</summary>
        [EnumMember(Value = "pending")]
        Pending = 0,

        /// <summary>Activated after successful subscription</summary>
        [EnumMember(Value = "activated")]
        Activated = 1,

        /// <summary>Subscription payment failed</summary>
        [EnumMember(Value = "failed")]
        Failed = 2,
    }

    /// <summary>
    /// State options for subscriptions 
    /// </summary>
    public enum SearchSubscriptionState
    {
        /// <summary>Active subscription</summary>
        [EnumMember(Value = "active")]
        Active,

        /// <summary>Inactive subscription</summary>
        [EnumMember(Value = "inactive")]
        Inactive
    }
}