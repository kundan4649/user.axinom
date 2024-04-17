using Newtonsoft.Json;
using z5.ms.domain.user.viewmodels;

namespace z5.ms.domain.user.user
{
    /// <summary>Common request model for user login</summary>
    public class LoginUserCommand
    {
        /// <summary>Type of the authentication method</summary>
        [JsonIgnore]
        public virtual AuthenticationMethod Type { get; set; }

        /// <summary>Country of the user acquired from IP</summary>
        [JsonIgnore]
        public string Country { get; set; }

        /// <summary>Indicates if refresh token is requested</summary>
        [JsonIgnore]
        public bool Refresh { get; set; } = true;
    }
}