using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace z5.ms.domain.subscription.voucher.viewmodels
{
    /// <summary>Voucher batch view model</summary>
    public class VoucherBatch
    {
        /// <summary> Id of the voucher batch</summary>
        [JsonProperty("batch_id", Required = Required.Always)]
        public string BatchId { get; set; }

        /// <summary> Description of the voucher batch</summary>
        [JsonProperty("batch_description", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string BatchDescription { get; set; }

        /// <summary>The total number of generated vouchers for the batch.</summary>
        [JsonProperty("num_of_generated", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public int NumOfGenerated { get; set; }

        /// <summary>The number of redeemed vouchers for the batch.</summary>
        [JsonProperty("num_of_redeemed", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public int NumOfRedeemed { get; set; }

        /// <summary> Start date of the validity period for voucher </summary>
        [JsonProperty("valid_from", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? ValidFrom { get; set; }

        /// <summary> End date of the validity period for voucher </summary>
        [JsonProperty("valid_until", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? ValidUntil { get; set; }

        /// <summary> Name of the CMS user who initiated generating vouchers. </summary>
        [JsonProperty("generated_by", Required = Required.Always)]
        public string GeneratedBy { get; set; }
    }

    /// <summary>Sort by enumerated values for voucher batches query</summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum VoucherBatchSortByField
    {
        /// <summary>BatchId</summary>
        [EnumMember(Value = "batch_id")]
        BatchId = 0,

        /// <summary>BatchDescription</summary>
        [EnumMember(Value = "batch_description")]
        BatchDescription = 1,

        /// <summary>NumOfGenerated</summary>
        [EnumMember(Value = "num_of_generated")]
        NumOfGenerated = 2,

        /// <summary>NumOfRedeemed</summary>
        [EnumMember(Value = "num_of_redeemed")]
        NumOfRedeemed = 3,

        /// <summary>ValidFrom</summary>
        [EnumMember(Value = "valid_from")]
        ValidFrom = 4,

        /// <summary>ValidUntil</summary>
        [EnumMember(Value = "valid_until")]
        ValidUntil = 5,

        /// <summary>GeneratedBy</summary>
        [EnumMember(Value = "generated_by")]
        GeneratedBy = 6
    }
}
