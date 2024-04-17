using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using z5.ms.domain.subscription.subscriptionplan;
using z5.ms.domain.subscription.viewmodel;

namespace z5.ms.domain.subscription.datamodel
{
    /// <summary>
    /// DB DTO for a subscription
    /// </summary>
    [Table("Subscriptions")]
    public class SubscriptionEntity
    {
        /// <summary>Unique ID of the subscription</summary>
        [Key]
        public Guid Id { get; set; }

        /// <summary>ID of the user who is associated with the subsciption</summary>
        [ForeignKey("Subscriber")]
        public Guid UserId { get; set; }

        /// <summary>Subscriber entity this subscription is linked to</summary>
        public SubscriberEntity Subscriber { get; set; }

        /// <summary>The external identifier like a subscription ID or similar of the related subscription in the payment providers system</summary>
        public string Identifier { get; set; }
        
        /// <summary>ID of the subscription plan this subscription is linked to</summary>
        public string SubscriptionPlanId { get; set; }
        
        /// <summary>Subscription start date</summary>
        public DateTime? SubscriptionStart { get; set; }
        
        /// <summary>Subscription end date</summary>
        public DateTime? SubscriptionEnd { get; set; }
        
        /// <summary>Flag indicating whether the subscription is recurring</summary>
        public bool RecurringEnabled { get; set; }

        /// <summary>Name of the payment provider associated with the subscription, all payment requests will be done using using this payment provider</summary>
        public string PaymentProviderName { get; set; }

        /// <summary>State of the subscription</summary>
        public SubscriptionState State { get; set; }

        /// <summary>Timestamp of the last subscription state change e</summary>
        public DateTime? StateChanged { get; set; }

        /// <summary>The associated payments</summary>
        public List<PaymentEntity> Payments { get; set; }

        /// <summary>The number of free trial days for this subscription</summary>
        public int FreeTrial { get; set; }

        /// <summary>Creation date</summary>
        public DateTime? Created { get; set; }

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

        /// <summary>
        /// The promo code used for this subscription.
        /// </summary>
        public string PromoCode { get; set; }
        
        /// <summary>
        /// The maximum number of billing cycles that the promo code can be applied for.
        /// </summary>
        public int AllowedBillingCycles { get; set; }
        
        /// <summary>
        /// The amount of billing cycled that have already been billed using the discount from the promotion.
        /// </summary>
        public int UsedBillingCycles { get; set; }
        
        
        
        /// <summary>
        /// Gets the discounted amount for this subscription if the discount hasn't been used more than allowed.
        /// </summary>
        public double? GetDiscountAmount() => UsedBillingCycles < AllowedBillingCycles ? DiscountAmount : 0d;
        
        /// <summary>Indicates if the subscription is active currently</summary>
        public bool IsActive => SubscriptionEnd != null && DateTime.UtcNow.Date <= SubscriptionEnd;

        /// <summary>Format a message for appending to the notes field</summary>
        public static string FormatNote(string msg) => $"{DateTime.UtcNow:dd/MM/yy HH:mm} - {msg}\n";

        /// <summary>Append a message to the notes field</summary>
        public void AppendNote(string msg) => Notes = (Notes ?? "") + FormatNote(msg);

        /// <summary>Set subscription dates on activation or renewal</summary>
        /// /// <param name="plan">The subscription plan being used to activate / renew the subscription</param>
        /// <param name="explicitStart">An optional start date is explicitly set by the payment provider</param>
        /// <param name="explicitEnd">An optional end date explicitly set by the payment provider</param>
        /// <param name="timeZoneOffset">Timezone offset of the location that subscription is activated (default is 0)</param>
        public void SetActivationDates(SubscriptionPlanEntity plan, DateTime? explicitStart, DateTime? explicitEnd, double timeZoneOffset = 0)
        {
            // note: if the subscription expired before yesterday then we will assume this is not a late billing.. its a historically expired subscription
            var isRecurringBilling = SubscriptionEnd != null && RecurringEnabled &&
                                     SubscriptionEnd >= DateTime.UtcNow.Date.AddDays(-1);

            var calculateEndDateFrom = (isRecurringBilling ? SubscriptionEnd.Value : explicitStart?.Date ?? DateTime.UtcNow.Date)
                .AddHours(timeZoneOffset);

            SubscriptionEnd = explicitEnd?.Date ?? (FreeTrial > 0
                                      ? calculateEndDateFrom.AddDays(FreeTrial)
                                      : plan.CalculateEndDateTime(calculateEndDateFrom));

            SubscriptionStart = explicitStart ??
                                    (isRecurringBilling ? SubscriptionStart : DateTime.UtcNow);
        }
    }
}
