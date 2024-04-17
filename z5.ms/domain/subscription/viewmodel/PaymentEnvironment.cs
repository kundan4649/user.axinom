using System.Runtime.Serialization;

namespace z5.ms.domain.subscription.viewmodel
{
    /// <summary>The environment type (e.g. web or app)</summary>
    public enum PaymentEnvironment
    {
        /// <summary>Unknown environment</summary>
        Unknown,

        /// <summary>This payment request originated from a mobile app</summary>
        /// <remarks>The default value</remarks>
        [EnumMember(Value = "app")]
        App,

        /// <summary>This payment request originated from the website</summary>
        [EnumMember(Value = "web")]
        Web
    }
}