using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using z5.ms.common.attributes;

namespace z5.ms.domain.subscription.viewmodel
{
    /// <summary>Request model make refund endpoint</summary>
    public class MakeRefund
    {
        /// <summary>The amount to be refunded</summary>
        [JsonProperty("amount")]
        [Required]
        [GreaterThan(0)]
        public double Amount { get; set; }

        /// <summary>Refund action to be applied on associated subscription</summary>
        [JsonProperty("action")]
        [Required]
        [EnumRange]
        public RefundAction Action { get; set; }

        /// <summary>User name of the person who makes te refund request (eg. CMS username)</summary>
        [JsonProperty("requester")]
        public string Requester { get; set; }

        /// <summary>Optional comment to provide information regarding the purpose of the refund</summary>
        [JsonProperty("comment")]
        public string Comment { get; set; }
    }

    /// <summary>Refund action enumeration to be applied on associated subscription</summary>
    public enum RefundAction
    {
        /// <summary>Leave the subscription as it is</summary>
        [EnumMember(Value = "do_nothing")]
        DoNothing = 0,

        /// <summary>Disable subscription renewal, so it'll be expired on next billing period</summary>
        [EnumMember(Value = "cancel_renewal")]
        CancelRenewal = 1,

        /// <summary>Cancel subscription immediately, so user can't access the premium content right after refund</summary>
        [EnumMember(Value = "cancel_immediately")]
        CancelImmediately = 2
    }
}
