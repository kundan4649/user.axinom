using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace z5.ms.domain.subscription.entitlement.viewmodel
{
    /// <summary>
    /// Response to the entitlement query
    /// </summary>
    [Obsolete("Use entitlement2")]
    public class EntitlementResponse 
    {
        /// <summary>Indicates whether a user is entitled to view an asset (used for only checking)</summary>
        [JsonProperty("entitled", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public bool? Entitled { get; set; }
    
        /// <summary>Generated DRM token for the asset (if requested)</summary>
        [JsonProperty("drm", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string Drm { get; set; }
    
        /// <summary>Generated CDN token(s) for the asset (if requested)</summary>
        [JsonProperty("cdn", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public List<CdnToken> Cdn { get; set; }
    }
    
}