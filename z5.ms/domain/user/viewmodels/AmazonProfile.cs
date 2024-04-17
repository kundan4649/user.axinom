using Newtonsoft.Json;

namespace z5.ms.domain.user.viewmodels
{
    /// <summary>User information model from Amazon for registration and login</summary>
    public class AmazonProfile
    {
        /// <summary>Unique Amazon user id</summary>
        [JsonProperty("user_id", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string UserId  { get; set; }

        /// <summary>User name from Amazon</summary>
        [JsonProperty("name", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        /// <summary>User email from Amazon</summary>
        [JsonProperty("email", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string Email { get; set; }
    }
}
