using Newtonsoft.Json;

namespace z5.ms.domain.entitlement4.viewmodel
{
    /// <summary>
    /// Container for a CDN token in the entitlement response
    /// </summary>
    public class CdnToken 
    {
        /// <summary>Name of the CDN token provider</summary>
        [JsonProperty("type", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string Type { get; set; }

        /// <summary>Actual token</summary>
        [JsonProperty("token", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string Token { get; set; }
    }
}