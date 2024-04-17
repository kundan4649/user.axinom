using System.Runtime.Serialization;

namespace z5.ms.common.assets
{
    /// <summary>Defines the unit of time that a subscription plan's billing frequency uses</summary>
    public enum BillingCycleType
    {
        /// <summary>Billing frequency is defined as a number of hours. Valid for Tvod only</summary>
        [EnumMember(Value = "hours")]
        Hours,

        /// <summary>Billing frequency is defined as a number of days. This is the default</summary>
        [EnumMember(Value = "days")]
        Days,

        /// <summary>Billing frequency is defined as a number of months.</summary>
        [EnumMember(Value = "months")]
        Months
    }
}