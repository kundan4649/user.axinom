using Newtonsoft.Json;

namespace z5.ms.domain.user.viewmodels
{
    /// <summary>User information model from social media for registration and login</summary>
    public class SocialProfile
    {
        /// <summary>Unique social media user id</summary>
        [JsonProperty("id", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string Id  { get; set; }

        /// <summary>User first name from social media</summary>
        [JsonProperty("first_name", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string FirstName { get; set; }

        /// <summary>User last name from social media</summary>
        [JsonProperty("last_name", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string LastName { get; set; }

        /// <summary>User email from social media</summary>
        [JsonProperty("email", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string Email { get; set; }
    }
}
