using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

namespace z5.ms.domain.user.viewmodels
{
    /// <summary>User view model</summary>
    public class User
    {
        /// <summary>The unique ID of the user</summary>
        [JsonProperty("id", Required = Required.Always)]
        [Required]
        public Guid Id { get; set; }

        /// <summary>User system</summary>
        [JsonProperty("system", Required = Required.Always)]
        [Required]
        public string System { get; set; }

        /// <summary>The email address of the user (if available)</summary>
        [JsonProperty("email", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string Email { get; set; }

        /// <summary>Indicates if email of the user is verified or not</summary>
        [JsonProperty("email_verified", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public bool? EmailVerified { get; set; }

        /// <summary>The mobile phone number of the user (if available)</summary>
        [JsonProperty("mobile", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string Mobile { get; set; }

        /// <summary>Indicates if mobile number of the user is verified or not</summary>
        [JsonProperty("mobile_verified", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public bool? MobileVerified { get; set; }

        /// <summary>The first name of the user</summary>
        [JsonProperty("first_name", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string FirstName { get; set; }

        /// <summary>The last name of the user</summary>
        [JsonProperty("last_name", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string LastName { get; set; }

        /// <summary>Some mac address - e.g. the one from the device the user registered with.</summary>
        [JsonProperty("mac_address", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string MacAddress { get; set; }

        /// <summary>The date when the user was born</summary>
        [JsonProperty("birthday", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? Birthday { get; set; }

        /// <summary>Gender</summary>
        [JsonProperty("gender", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(StringEnumConverter))]
        public Gender? Gender { get; set; }

        /// <summary>The date and time when the user account was activated</summary>
        [JsonProperty("activation_date", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? ActivationDate { get; set; }

        /// <summary>If the user is activated or not</summary>
        [JsonProperty("activated", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public bool? Activated { get; set; }

        /// <summary>IP address of the user</summary>
        [JsonProperty("ip_address", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string IpAddress { get; set; }

        /// <summary>Country in "ISO 3166-1 alpha-2" format from where the user initially registered.</summary>
        [JsonProperty("registration_country", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        [StringLength(2, MinimumLength = 2)]
        public string RegistrationCountry { get; set; }

        /// <summary>The specific region in the registration country.</summary>
        [JsonProperty("registration_region", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string RegistrationRegion { get; set; }

        /// <summary>Additional information for user to be stored in DB for reporting purposes</summary>
        [JsonProperty("additional", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public JObject Additional { get; set; }

        /// <summary>Timestamp of last user login</summary>
        [JsonProperty("is_password_attached", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public bool IsPasswordAttached { get; set; }
    }
}
