using Newtonsoft.Json;

namespace z5.ms.domain.user.viewmodels
{
    /// <summary>Google access token response model</summary>
    public class GoogleTokenResponse
    {
        /// <summary>Access token provided by google for getting user profile</summary>
        [JsonProperty("access_token", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string AccessToken { get; set; }

        /// <summary>Token expiration time</summary>
        [JsonProperty("expires_in", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public long ExpiresIn { get; set; }

        /// <summary>JWT token </summary>
        [JsonProperty("id_token", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string IdToken { get; set; }

        /// <summary>Indicates token type it should be "Bearer"</summary>
        [JsonProperty("token_type", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string TokenType { get; set; }

        /// <summary>Error title</summary>
        [JsonProperty("error", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string Error { get; set; }

        /// <summary>Error description</summary>
        [JsonProperty("error_description", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string ErrorDescription { get; set; }
    }
}
