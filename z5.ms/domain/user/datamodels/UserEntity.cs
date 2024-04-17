using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using z5.ms.domain.user.viewmodels;

namespace z5.ms.domain.user.datamodels
{
    /// <summary>Active state of a user account</summary>
    public enum UserState
    {
        /// <summary>User has registered, but not confirmed their email / mobile number</summary>
        Registered = 1,

        /// <summary>After confirming e-mail, phone number</summary>
        Verified = 10,

        /// <summary>Account is deleted</summary>
        Deleted = 99
    }

    /// <summary>Database entity type for User</summary>
    [Table("Users")]
    public class UserEntity : ICloneable
    {
        /// <summary>The unique ID of the user</summary>
        [Key]
        public Guid Id { get; set; }

        /// <summary>Active state of the user's account</summary>
        public UserState State { get; set; }

        /// <summary>The email address of the user (if available)</summary>
        public string Email { get; set; }

        /// <summary>The mobile phone number of the user (if available)</summary>
        public string Mobile { get; set; }

        /// <summary>The first name of the user</summary>
        public string FirstName { get; set; }

        /// <summary>The last name of the user</summary>
        public string LastName { get; set; }

        /// <summary>The password hash of user</summary>
        /// <remarks>This field may contain multiple values separated by a separator e.g. hash + salt</remarks>
        public string PasswordHash { get; set; }

        /// <summary>Random generated key to be sent to users email address to confirm that</summary>
        public string EmailConfirmationKey { get; set; }

        /// <summary>Random generated key to be sent to users phone number to confirm that</summary>
        public string MobileConfirmationKey { get; set; }

        /// <summary>Expiration date for email confirmation key</summary>
        public DateTime? EmailConfirmationExpiration { get; set; }

        /// <summary>Expiration date for mobile confirmation key</summary>
        public DateTime? MobileConfirmationExpiration { get; set; }

        /// <summary>Boolean to indicate email confirmation status</summary>
        public bool IsEmailConfirmed { get; set; }

        /// <summary>Boolean to indicate mobile number confirmation status</summary>
        public bool IsMobileConfirmed { get; set; }

        /// <summary>Random generated key to be sent to user's mail/mobile to reset password</summary>
        public string PasswordResetKey { get; set; }

        /// <summary>Expiration date for password reset key</summary>
        public DateTime? PasswordResetExpiration { get; set; }

        /// <summary>Timestamp of last user login</summary>
        public DateTime? LastLogin { get; set; }

        /// <summary>User system</summary>
        public string System { get; set; }

        /// <summary>Country in “ISO 3166-1 alpha-2” format from where the user initially registered.</summary>
        public string RegistrationCountry { get; set; }

        /// <summary>Registration region of the user from Maxmind DB</summary>
        public string RegistrationRegion { get; set; }

        /// <summary>Mac address of user</summary>
        public string MacAddress { get; set; }

        /// <summary>The date when the user was born</summary>
        public DateTime? Birthday { get; set; }

        /// <summary>Gender</summary>
        public Gender? Gender { get; set; }

        /// <summary>The date and time when the user account was activated</summary>
        public DateTime? ActivationDate { get; set; }

        /// <summary>The date and time when the user account was created</summary>
        public DateTime? CreationDate { get; set; }

        /// <summary>Facebook user id of user</summary>
        public string FacebookUserId { get; set; }

        /// <summary>Google user id of user</summary>
        public string GoogleUserId { get; set; }

        /// <summary>Twitter user id of user</summary>
        public string TwitterUserId { get; set; }

        /// <summary>Amazon user id of user</summary>
        public string AmazonUserId { get; set; }

        /// <summary>B2B user id of user</summary>
        public string B2BUserId { get; set; }

        /// <summary>Updated email for the user, awiting confirmation</summary>
        public string NewEmail { get; set; }

        /// <summary>Random generated key to be sent to users email address to confirm change</summary>
        public string NewEmailConfirmationKey { get; set; }

        /// <summary>Expiration date for email confirmation key</summary>
        public DateTime? NewEmailConfirmationExpiration { get; set; }

        /// <summary>IP address of the user</summary>
        public string IpAddress { get; set; }

        /// <summary>Additional information for user to be stored in DB for reporting purposes</summary>
        public string Json { get; set; }

        /// <summary>External identity provider name.</summary>
        public string ProviderName { get; set; }
        
        /// <summary>Subject identifier from the external provider.</summary>
        public string ProviderSubjectId { get; set; }

        /// <summary>Duplicate social profileIds</summary>
        public string DuplicateProfileIds { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
