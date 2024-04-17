using System;
using Newtonsoft.Json;

namespace z5.ms.common.validation
{
    /// <summary>
    /// Authentication token model definition
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class AuthToken
    {
        /// <summary>The unique ID of the user</summary>
        [JsonProperty("user_id", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public Guid UserId { get; set; }

        /// <summary>The system the user is authenticated with</summary>
        [JsonProperty("system", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string System { get; set; }

        /// <summary>The email address of the user (if available)</summary>
        [JsonProperty("user_email", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string UserEmail { get; set; }

        /// <summary>The mobile phone number of the user (if available)</summary>
        [JsonProperty("user_mobile", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string UserMobile { get; set; }

        /// <summary>The date and time when the user account was activated</summary>
        [JsonProperty("activation_date", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public DateTime ActivationDate { get; set; }

        /// <summary>The date and time when the token is created</summary>
        [JsonProperty("created_date", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public DateTime CreatedDate { get; set; }

        /// <summary>The registration country of the user (if available)</summary>
        [JsonProperty("registration_country", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string RegistrationCountry { get; set; }
    }
}
