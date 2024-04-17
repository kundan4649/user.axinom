using System;
using Newtonsoft.Json;

namespace z5.ms.domain.subscription.entitlement.viewmodel
{
    /// <summary>
    /// Container for a CDN token in the entitlement response
    /// </summary>
    [Obsolete("Use entitlement2")]
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