using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace z5.ms.domain.subscription.voucher.datamodels
{
    /// <summary> Database entity type for Voucher </summary>
    [Table("Vouchers")]
    public class VoucherEntity
    {
        /// <summary> Voucher code </summary>
        [Key]
        public string VoucherCode { get; set; }

        /// <summary> Id of the voucher batch</summary>
        public string BatchId { get; set; }        
        
        /// <summary> Voucher redeem status (reedeemed or not) </summary>
        public bool? Redeemed { get; set; }

        /// <summary> Date of the voucher to be redeemed </summary>
        public DateTime? RedeemedDate { get; set; }

        /// <summary> The unique ID of the user who redeemed the voucher code </summary>
        public Guid? RedeemedBy { get; set; }
    }
}
