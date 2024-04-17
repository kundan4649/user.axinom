using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace z5.ms.domain.subscription.voucher.datamodels
{
    /// <summary> Database entity type for Voucher </summary>
    [Table("VouchersBatches")]
    public class VouchersBatchEntity
    {
        /// <summary> Voucher Batch Id</summary>
        [Key]
        public string Id { get; set; }

        /// <summary> Description of the voucher batch</summary>
        public string BatchDescription { get; set; }

        /// <summary> The ID of the associated subscription plan </summary>
        public string SubscriptionPlanId { get; set; }

        /// <summary> Start date of the validity period for voucher </summary>
        public DateTime? ValidFrom { get; set; }

        /// <summary> End date of the validity period for voucher </summary>
        public DateTime? ValidUntil { get; set; }

        /// <summary> Generated date of the voucher </summary>
        public DateTime GeneratedDate { get; set; }

        /// <summary> Name of the CMS user who initiated generating vouchers. </summary>
        public string GeneratedBy { get; set; }

        /// <summary> Switch to turn off/on usage of uppercase letters (ascii uppercase) in generating vouchers. </summary>
        public bool UseUppercase { get; set; }

        /// <summary> Switch to turn off/on usage of lowercase  letters (ascii lowercase) in generating vouchers. </summary>
        public bool UseLowercase { get; set; }

        /// <summary> Switch to turn off/on usage of numbers (ascii digits) in generating vouchers. </summary>
        public bool UseNumbers { get; set; }

        /// <summary> Characters that not to be used in voucher generation. </summary>
        public string NotToUseCharacters { get; set; }

        /// <summary> The pattern to be used in generation of vouchers ("PREFIX-####-####-SUFFIX" only "#" characters will be replaced according to generation rules) </summary>
        public string VoucherPattern { get; set; }
    }
}
