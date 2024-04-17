using System.Runtime.Serialization;

namespace z5.ms.domain.subscription.viewmodel
{
    /// <summary>
    /// State options for purchases
    /// </summary>
    public enum PurchaseState
    {
        /// <summary>Purchase waiting for payment</summary>
        [EnumMember(Value = "pending")]
        Pending = 0,

        /// <summary>Activated after successful purchase</summary>
        [EnumMember(Value = "activated")]
        Activated = 1,

        /// <summary>Purchase payment failed</summary>
        [EnumMember(Value = "failed")]
        Failed = 2
    }

    /// <summary>
    /// State options for purchases 
    /// </summary>
    public enum SearchPurchaseState
    {
        /// <summary>Active purchase</summary>
        [EnumMember(Value = "active")]
        Active,

        /// <summary>Inactive purchase</summary>
        [EnumMember(Value = "inactive")]
        Inactive
    }
}