using Newtonsoft.Json;

namespace z5.ms.domain.user.viewmodels
{
    /// <summary>User information model from facebook for registration and login</summary>
    public class FacebookProfile
    {
        /// <summary>Unique Facebook user id</summary>
        [JsonProperty("id", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string Id  { get; set; }

        /// <summary>User first name from facebook</summary>
        [JsonProperty("first_name", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string FirstName { get; set; }

        /// <summary>User last name from facebook</summary>
        [JsonProperty("last_name", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string LastName { get; set; }

        /// <summary>User email from facebook</summary>
        [JsonProperty("email", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string Email { get; set; }
    }
}
