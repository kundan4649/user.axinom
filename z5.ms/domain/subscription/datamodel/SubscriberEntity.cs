using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace z5.ms.domain.subscription.datamodel
{
    /// <summary>Database entity type for User</summary>
    [Table("Subscribers")]
    public class SubscriberEntity
    {
        /// <summary>The unique ID of the user</summary>
        [Key]
        public Guid Id { get; set; }

        /// <summary>The email address of the user (if available)</summary>
        public string Email { get; set; }

        /// <summary>The mobile phone number of the user (if available)</summary>
        public string Mobile { get; set; }

        /// <summary>The first name of the user</summary>
        public string FirstName { get; set; }

        /// <summary>The last name of the user</summary>
        public string LastName { get; set; }

        /// <summary>User system</summary>
        public string System { get; set; }

        /// <summary>Country in “ISO 3166-1 alpha-2” format from where the user initially registered.</summary>
        public string RegistrationCountry { get; set; }

        /// <summary>Boolean to indicate user is deleted or not</summary>
        public bool IsDeleted { get; set; }

        /// <summary>Time the last event to used update this entity was created</summary>
        public DateTime Updated { get; set; }

        /// <summary>The user's subscriptions</summary>
        public List<SubscriptionEntity> Subscriptions { get; set; }

        /// <summary>The user's purchases</summary>
        public List<PurchaseEntity> Purchases { get; set; }

        /// <summary>The user's donations</summary>
        public List<DonationEntity> Donations { get; set; }

    }
}
