using Newtonsoft.Json;

namespace z5.ms.domain.user.viewmodels
{
    /// <summary>The JWT of the user containing the user ID, email, mobile phone and activation date</summary>
    public class Token
    {
        /// <summary>JWT string from serialized authentication token</summary>
        [JsonProperty("token", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string AuthToken { get; set; }
    }
}
