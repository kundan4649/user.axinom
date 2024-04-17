using Newtonsoft.Json;

namespace z5.ms.domain.user.viewmodels
{
    /// <summary>User information model from twitter for registration and login</summary>
    public class TwitterProfile
    {
        /// <summary>Unique Google user id</summary>
        [JsonProperty("id", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string Id  { get; set; }

        /// <summary>User name from twitter (includes full name)</summary>
        [JsonProperty("name", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        /// <summary>User email from twitter</summary>
        [JsonProperty("email", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string Email { get; set; }
    }
}
