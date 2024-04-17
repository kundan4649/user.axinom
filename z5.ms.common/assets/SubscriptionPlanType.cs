using System.Runtime.Serialization;

namespace z5.ms.common.assets
{
    /// <summary>The available subscription types</summary>
    public enum SubscriptionPlanType : short
    {
        /// <summary>Subscription Video On Demand; subscription purchases</summary>
        [EnumMember(Value = "SVOD")]
        Svod,

        /// <summary>Subscription Video On Demand; free</summary>
        [EnumMember(Value = "FVOD")]
        Fvod,

        /// <summary>Subscription Video On Demand; advertisement supported</summary>
        [EnumMember(Value = "AVOD")]
        Avod,

        /// <summary>Transactional Video On Demand; individual asset purchases</summary>
        [EnumMember(Value = "TVOD")]
        Tvod,

        /// <summary>Donation; a payment without any entitlement</summary>
        [EnumMember(Value = "Donation")]
        Donation
    }
}