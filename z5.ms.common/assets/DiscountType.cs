using System.Runtime.Serialization;

namespace z5.ms.common.assets
{
    /// <summary>Defines the type of the discount to be applied on the subscription plan price</summary>
    public enum DiscountType
    {
        /// <summary>Direct discount from the price</summary>
        [EnumMember(Value = "currency")]
        Currency,

        /// <summary>Percentage of the discount on the price</summary>
        [EnumMember(Value = "percentage")]
        Percentage
    }
}