using System.Collections.Generic;
using Newtonsoft.Json;

namespace z5.ms.domain.user.viewmodels
{
    /// <summary>User information model from google for registration and login</summary>
    public class GoogleProfile
    {
        /// <summary>Unique Google user id</summary>
        [JsonProperty("id", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string Id  { get; set; }

        /// <summary>User name from google</summary>
        [JsonProperty("name", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public GoogleName Name { get; set; }

        /// <summary>User email from google</summary>
        [JsonProperty("emails", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public List<GoogleEmail> Emails { get; set; }
    }

    /// <summary>User name model of google</summary>
    public class GoogleName
    {
        /// <summary>First name of user</summary>
        [JsonProperty("givenName", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string FirstName { get; set; }

        /// <summary>Last name of user</summary>
        [JsonProperty("familyName", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string LastName { get; set; }
    }

    /// <summary>Email model of google</summary>
    public class GoogleEmail
    {
        /// <summary>Email address of user</summary>
        [JsonProperty("value", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string Email { get; set; }

        /// <summary>Type of email</summary>
        [JsonProperty("type", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string Type { get; set; }
    }
}
