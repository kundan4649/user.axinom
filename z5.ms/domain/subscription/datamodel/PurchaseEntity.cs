using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using z5.ms.domain.subscription.viewmodel;

namespace z5.ms.domain.subscription.datamodel
{
    /// <summary>
    /// DB DTO for a purchase 
    /// </summary>
    [Table("Purchases")]
    public class PurchaseEntity
    {
        /// <summary>Unique id of the purchase</summary>
        [Key]
        public Guid Id { get; set; }

        /// <summary>FK linking to the user/customer who made the purchase</summary>
        [ForeignKey("Subscriber")]
        public Guid UserId { get; set; }

        /// <summary>Subscriber entity this purchase is linked to</summary>
        public SubscriberEntity Subscriber { get; set; }

        /// <summary>The external identifier like a purchase ID or similar of the related subscription in the payment providers system</summary>
        public string Identifier { get; set; }

        /// <summary>ID of the asset that was purchased</summary>
        public string AssetId { get; set; }
        
        /// <summary>ID of the subscription plan that was purchased</summary>
        public string SubscriptionPlanId { get; set; }
       
        /// <summary>Name of the payment provider used</summary>
        public string PaymentProviderName { get; set; }

        /// <summary>State of the subscription</summary>
        public PurchaseState State { get; set; }

        /// <summary>Timestamp of the last subscription state change e</summary>
        public DateTime? StateChanged { get; set; }

        /// <summary>The date at which point the purchase ends. Null for infinite length</summary>
        public DateTime? PurchaseEnd { get; set; }

        /// <summary>Creation date</summary>
        public DateTime Date { get; set; }

        /// <summary>The associated payment</summary>
        public PaymentEntity Payment { get; set; }

        /// <summary>Is this purchase expired</summary>
        public bool IsExpired => PurchaseEnd != null && PurchaseEnd <= DateTime.UtcNow;

        /// <summary>The last payment identifier associated with this subscription</summary>
        public string LastTransactionIdentifier { get; set; }

        /// <summary>Success and error notes about subscription history</summary>
        public string Notes { get; set; }

        /// <summary>The absolute amount that this subscription is discounted by</summary>
        public double? DiscountAmount { get; set; }

        /// <summary>IP address of the user</summary>
        public string IpAddress { get; set; }

        /// <summary>Country in “ISO 3166-1 alpha-2” format from where the user initially registered.</summary>
        public string RegistrationCountry { get; set; }

        /// <summary>Registration region of the user from Maxmind DB</summary>
        public string RegistrationRegion { get; set; }

        /// <summary>Additional information for user to be stored in DB for reporting purposes</summary>
        public string Json { get; set; }

        /// <summary>Format a message for appending to the notes field</summary>
        public static string FormatNote(string msg) => $"{DateTime.UtcNow:dd/MM/yy HH:mm} - {msg}\n";

        /// <summary>Append a message to the notes field</summary>
        public void AppendNote(string msg) => Notes = (Notes ?? "") + FormatNote(msg);
    }
}
