using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using z5.ms.domain.subscription.payment;

namespace z5.ms.domain.subscription.datamodel
{
    //TODO: review payment linking to subscriptions/purchases, having two optional FKs seems a bit fishy
    /// <summary>
    /// DB DTO for a single payment
    /// </summary>
    /// <remarks>Can link to either a subscription or a purchase</remarks>
    [Table("Payments")]
    public class PaymentEntity
    {
        /// <summary>Unique ID of the payment</summary>
        [Key]
        public Guid Id { get; set; }

        /// <summary>FK to the subscription that this payment is linked to</summary>
        [ForeignKey("Subscription")]
        public Guid? SubscriptionId { get; set; }

        /// <summary>Subscription object this payment is linked to</summary>
        public SubscriptionEntity Subscription { get; set; }

        /// <summary>FK to the purchase that this payment is linked to</summary>
        [ForeignKey("Purchase")]
        public Guid? PurchaseId { get; set; }
        
        /// <summary>Purchase object this payment is linked to</summary>
        public PurchaseEntity Purchase { get; set; }

        /// <summary>FK to the donation this payment is linked to</summary>
        [ForeignKey("Donation")]
        public Guid? DonationId { get; set; }

        /// <summary>Donation entity this payment is linked to</summary>
        public DonationEntity Donation { get; set; }

        /// <summary>The amount that was paid</summary>
        public double Amount { get; set; }

        /// <summary>The absolute amount that this subscription is discounted by</summary>
        public double? DiscountAmount { get; set; }

        /// <summary>Currency used during the payment</summary>
        public string Currency { get; set; }
        
        /// <summary>Taxes applied to the payment.</summary>
        /// <remarks>Serialized JSON: 
        /// {
        ///    "name": "Service Tax",
        ///    "percentage": 14.0
        ///}
        /// </remarks>
        public string Taxes { get; set; }
        
        // TODO: find a more expressive name
        /// <summary>External identifier that is provided by a payment provider, e.g. transaction ID</summary>
        public string Identifier { get; set; }
        
        /// <summary>Additional details for the used payment provider e.g. card brand for Gateway billing or mobile carrier name for Carrier billing</summary>
        public string Description { get; set; }
        
        /// <summary>Date when the payment was made</summary>
        public DateTime Date { get; set; }

        /// <summary>State of a payment</summary>
        public PaymentState State { get; set; }

        /// <summary>Timestamp of the last payment state change</summary>
        public DateTime? StateChanged { get; set; }

        /// <summary>The unique ID of the customer who made this payment</summary>
        public Guid? CustomerId => Subscription?.UserId ?? Purchase?.UserId ?? Donation?.UserId;

        /// <summary>The unique ID of the subscription plan used to make this payment</summary>
        public string SubscriptionPlanId => Subscription?.SubscriptionPlanId ?? Purchase?.SubscriptionPlanId ?? Donation?.SubscriptionPlanId;

        /// <summary>The ID of the payment provider used to make this payment</summary>
        public string PaymentProvider => Subscription?.PaymentProviderName ?? Purchase?.PaymentProviderName ?? Donation?.PaymentProviderName;

        /// <summary>Active payments are treated as paid. Inactive payments should be hidden from all search results</summary>
        public bool IsActive => State == PaymentState.Completed || State == PaymentState.PendingAccepted;
    }
}
