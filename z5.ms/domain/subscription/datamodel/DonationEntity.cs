using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace z5.ms.domain.subscription.datamodel
{
    /// <summary>DB DTO for a donation.</summary>
    [Table("Donations")]
    public class DonationEntity
    {
        /// <summary>Unique ID of the donation.</summary>
        [Key]
        public Guid Id { get; set; }

        /// <summary>FK to the user making the donation. If this is null, the donation is anonymous.</summary>
        [ForeignKey("Subscriber")]
        public Guid? UserId { get; set; }

        /// <summary>Subscriber entity this donation is linked to</summary>
        public SubscriberEntity Subscriber { get; set; }

        /// <summary>FK to the subscription plan that defines the context of the donation.</summary>
        public string SubscriptionPlanId { get; set; }

        /// <summary>The amount of money that was paid</summary>
        public double Amount { get; set; }

        /// <summary>The currency in which was paid.</summary>
        public string Currency { get; set; }

        /// <summary>Name of the payment provider used</summary>
        public string PaymentProviderName { get; set; }

        /// <summary>Creation date</summary>
        public DateTime Date { get; set; }

        /// <summary>The associated payment</summary>
        public PaymentEntity Payment { get; set; }
    }
}
