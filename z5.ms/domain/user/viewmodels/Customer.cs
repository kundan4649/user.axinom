using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using z5.ms.domain.user.datamodels;

namespace z5.ms.domain.user.viewmodels
{
    /// <summary>
    /// Customer model for internal usage between media server and CMS
    /// </summary>
    public class Customer
    {
        /// <summary>The unique ID of the customer</summary>
        [JsonProperty("id", Required = Required.Always)]
        [Required]
        public Guid Id { get; set; }

        /// <summary>The system from where the user registered from (DC or B2B customers).</summary>
        [JsonProperty("system", Required = Required.Always)]
        public string System { get; set; }

        /// <summary>The email address of the customer</summary>
        [JsonProperty("email", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string Email { get; set; }

        /// <summary>Indicates if email of the user is verified or not</summary>
        [JsonProperty("email_verified", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public bool? EmailVerified { get; set; }

        /// <summary>The mobile phone number of the customer</summary>
        [JsonProperty("mobile", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string Mobile { get; set; }

        /// <summary>Indicates if mobile number of the user is verified or not</summary>
        [JsonProperty("mobile_verified", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public bool? MobileVerified { get; set; }

        /// <summary>The facebook handle of the customer</summary>
        [JsonProperty("facebook_handle", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string FacebookUserId { get; set; }

        /// <summary>The google handle of the customer</summary>
        [JsonProperty("google_handle", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string GoogleUserId { get; set; }

        /// <summary>The twitter handle of the customer</summary>
        [JsonProperty("twitter_handle", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string TwitterUserId { get; set; }

        /// <summary>The amazon handle of the customer</summary>
        [JsonProperty("amazon_handle", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string AmazonUserId { get; set; }

        /// <summary>The first name of the customer</summary>
        [JsonProperty("first_name", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string FirstName { get; set; }

        /// <summary>The first name of the customer</summary>
        [JsonProperty("last_name", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string LastName { get; set; }

        /// <summary>Some mac address - e.g. the one from the device the user registered with.</summary>
        [JsonProperty("mac_address", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string MacAddress { get; set; }

        /// <summary>The date when the user was born</summary>
        [JsonProperty("birthday", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? Birthday { get; set; }

        /// <summary>The gender of the user</summary>
        [JsonProperty("gender", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(StringEnumConverter))]
        public Gender? Gender { get; set; }

        /// <summary>The date and time when the customer was created</summary>
        [JsonProperty("create_date", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? CreationDate { get; set; }

        /// <summary>The date and time when the customer was activated</summary>
        [JsonProperty("activation_date", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? ActivationDate { get; set; }

        /// <summary>If the user is activated or not</summary>
        [JsonProperty("activated", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public bool? Activated { get; set; }

        /// <summary>Timestamp of last user login</summary>
        [JsonProperty("last_login", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? LastLogin { get; set; }

        /// <summary>IP address of the user</summary>
        [JsonProperty("ip_address", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string IpAddress { get; set; }

        /// <summary>Country in “ISO 3166-1 alpha-2” format from where the user initially registered.</summary>
        [JsonProperty("registration_country", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        [StringLength(2)]
        public string RegistrationCountry { get; set; }

        /// <summary>The specific region in the registration country.</summary>
        [JsonProperty("registration_region", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string RegistrationRegion { get; set; }

        /// <summary>Additional information for user to be stored in DB for reporting purposes</summary>
        [JsonProperty("additional", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public JObject Additional { get; set; }

        /// <summary>Active state of the user's account</summary>
        [JsonProperty("state", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public UserState State { get; set; }
    }

    /// <summary>Sort by enumerated values for customers query</summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum CustomerSortByField
    {
        /// <summary>Email</summary>
        [EnumMember(Value = "email")]
        Email = 0,

        /// <summary>Firstname</summary>
        [EnumMember(Value = "first_name")]
        Firstname = 1,

        /// <summary>Lastname</summary>
        [EnumMember(Value = "last_name")]
        Lastname = 2,

        /// <summary>Lastlogin</summary>
        [EnumMember(Value = "last_login")]
        Lastlogin = 3,
    }
}
