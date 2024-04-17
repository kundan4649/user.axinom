using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace z5.ms.domain.subscription.datamodel
{
    /// <summary>Entity model for Refund</summary>
    [Table("Refunds")]
    public class RefundEntity
    {
        /// <summary>The unique database ID of the refund</summary>
        [Key]
        public Guid Id { get; set; }

        /// <summary>The unique id of the related payment</summary>
        public Guid PaymentId { get; set; }

        /// <summary>The amount to be refunded</summary>
        public double Amount { get; set; }

        /// <summary>The unique identifier of the refund from payment provider.</summary>
        public string ExternalIdentifier { get; set; }

        /// <summary>User name of the person who makes te refund request (eg. CMS username)</summary>
        public string Requester { get; set; }

        /// <summary>Optional comment to provide information regarding the purpose of the refund</summary>
        public string Comment { get; set; }

        /// <summary>The date and time when the refund is made</summary>
        public DateTime Date { get; set; }
    }
}
