using System;
using Newtonsoft.Json;

namespace z5.ms.domain.subscription.voucher.viewmodels
{ 
    /// <summary>Voucher view model</summary>
    public class Voucher
    {
        /// <summary> Voucher code </summary>
        [JsonProperty("voucher_code", Required = Required.Always)]
        public string VoucherCode { get; set; }

        /// <summary> The ID of the associated subscription plan </summary>
        [JsonProperty("subscription_plan_id", Required = Required.Always)]
        public string SubscriptionPlanId { get; set; }

        /// <summary> Id of the voucher batch</summary>
        [JsonProperty("batch_id", Required = Required.Always)]
        public string BatchId { get; set; }

        /// <summary> Description of the voucher batch</summary>
        [JsonProperty("batch_description", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string BatchDescription { get; set; }

        /// <summary> Start date of the validity period for voucher </summary>
        [JsonProperty("valid_from", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? ValidFrom { get; set; }

        /// <summary> End date of the validity period for voucher </summary>
        [JsonProperty("valid_until", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? ValidUntil { get; set; }

        /// <summary> Generated date of the voucher </summary>
        [JsonProperty("generated_date", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public DateTime GeneratedDate { get; set; }

        /// <summary> Name of the CMS user who initiated generating vouchers. </summary>
        [JsonProperty("generated_by", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string GeneratedBy { get; set; }

        /// <summary> Voucher redeem status (reedeemed or not) </summary>
        [JsonProperty("redeemed", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public bool Redeemed { get; set; }

        /// <summary> Date of the voucher to be redeemed </summary>
        [JsonProperty("redeemed_date", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? RedeemedDate { get; set; }

        /// <summary> The unique ID of the user who redeemed the voucher code </summary>
        [JsonProperty("redeemed_by", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public Guid? RedeemedBy { get; set; }
    }
}
