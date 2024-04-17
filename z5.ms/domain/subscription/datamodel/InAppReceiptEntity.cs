using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace z5.ms.domain.subscription.datamodel
{
    /// <summary>DB DTO for in-app purchase receipts.</summary>
    [Table("InAppReceipts")]
    public class InAppReceiptEntity
    {
        /// <summary>Subscription id of related in-app purchase</summary>
        [Key]
        public Guid SubscriptionId { get; set; }

        /// <summary>Receipt data for related in-app purchase</summary>
        public string Receipt { get; set; }
    }
}
