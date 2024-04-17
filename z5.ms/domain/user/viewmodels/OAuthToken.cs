using Newtonsoft.Json;

namespace z5.ms.domain.user.viewmodels
{
    /// <summary>The OAuth2 JWT of the user containing the user ID, email, mobile phone and activation date.</summary>
    public class OAuthToken
    {
        /// <summary>Id token of the user</summary>
        [JsonProperty("id_token", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string IdToken { get; set; }

        /// <summary>The short lived access token to access APIs</summary>
        [JsonProperty("access_token", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string AccessToken { get; set; }

        /// <summary>The time in seconds when the access token will expire.</summary>
        [JsonProperty("expires_in", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public long ExpiresIn { get; set; }

        /// <summary>The type of the access token - in our case always 'bearer'</summary>
        [JsonProperty("token_type", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string TokenType { get; set; } = "Bearer";

        /// <summary>The long lived refresh token to get a new access token once it expires.</summary>
        [JsonProperty("refresh_token", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string RefreshToken { get; set; }
    }
}
