using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using z5.ms.common.attributes;

namespace z5.ms.domain.user.viewmodels
{
    /// <summary>
    /// Customer model for internal usage between media server and CMS
    /// </summary>
    public class CustomerChangeDetails
    {
        /// <summary>The unique ID of the customer</summary>
        [JsonProperty("id", Required = Required.Always)]
        [Required]
        public Guid Id { get; set; }

        /// <summary>The system from where the user registered from.</summary>
        [JsonProperty("system", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string System { get; set; }

        /// <summary>Country in “ISO 3166-1 alpha-2” format from where the user initially registered.</summary>
        [JsonProperty("country", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        [StringLength(2)]
        public string RegistrationCountry { get; set; }

        /// <summary>The email address of the customer</summary>
        [JsonProperty("email", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        [EmailAddress]
        public string Email { get; set; }

        /// <summary>The mobile phone number of the customer</summary>
        [JsonProperty("mobile", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        [PhoneNumber]
        public string Mobile { get; set; }

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

        [JsonIgnore]
        /// <summary>Raw body</summary>
        public string RawRequest { get; set; }

        /// <summary>User Id from which the request is received. Shouldn't be accessible from API</summary>
        [JsonIgnore]
        public string IpAddress { get; set; }

        /// <summary>CountryCode from which the request is received. Shouldn't be accessible from API</summary>
        [JsonIgnore]
        public string CountryCode { get; set; }
    }
}
