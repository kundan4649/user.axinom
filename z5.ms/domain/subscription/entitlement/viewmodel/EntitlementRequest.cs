using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace z5.ms.domain.subscription.entitlement.viewmodel
{
    /// <summary>
    /// Request model for the entitlement API
    /// </summary>
    [Obsolete("Use entitlement2")]
    public class EntitlementRequest 
    {
        /// <summary>The asset ID of the item that should be played back. Supported are episodes, movies and channels (they have streams that can be played). But not TV shows, seasons, EPG programs, collections... (they do not have streams on their own).</summary>
        [JsonProperty("asset_id", Required = Required.Always)]
        [Required]
        public string AssetId { get; set; }
    
        /// <summary>
        /// Type of the entitlement provider:
        ///  * `internal` - uses our application logic (subscription &amp; purchase logic)
        ///  * `google` - specific logic for Google Playstore token based entitlement
        ///  * `apple` - specific logic for Apple App Store token based entitlement
        ///  * `cms` -  MS shall trust CMS, so if CMS asks for a certain token it shall get it. Shared secret should be used to authenticate CMS
        ///  * `trailer` - always return CDN tokens to access to "{movie_id}/trailer", don't check anything. No DRM token for trailers (if asked for DRM - error).
        /// </summary>
        [JsonProperty("entitlement_provider", Required = Required.Always)]
        [Required]
        public string EntitlementProvider { get; set; }
    
        // TODO: consider making this a string, it would allow to give reasonable error messages during validation instead of failing during parameter mapping with an obscure 'Object reference not set to an instance of an object'
        /// <summary>Type of the entitlement request/// </summary>
        [JsonProperty("request_type", Required = Required.Always)]
        [Required]
        [JsonConverter(typeof(StringEnumConverter))]
        public EntitlementRequestType RequestType { get; set; }
    
        /// <summary>The token to use for entitlement check, it may be a user token (internal subscription/purchase), Google Playstore token or an Apple App Store token</summary>
        [JsonProperty("token", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string Token { get; set; }
    
        // TODO: put external parameters in a dict so that they could be easily extended 
        /// <summary>An external ID that may be required to to verify external entilement providers (e.g. the ID of a Google playstore subscription product)</summary>
        [JsonProperty("ext_id", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string ExtId { get; set; }
    
        /// <summary>The content key ID of the protected video stream</summary>
        [JsonProperty("key_id", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string KeyId { get; set; }
    
        /// <summary>If the entitlement message should contain the flag to make the DRM license a persistent one.</summary>
        [JsonProperty("persistent", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public bool Persistent { get; set; }

        /// <summary>The current country of the user. Required for playback of premium content with licensing restrictions.</summary>
        private string Country { get; set; }

        public void SetCountry(string value) => Country = value;

        public string GetCountry() => Country;
    }

    /// <summary>Type of the entitlement request</summary>
    [Obsolete("Use entitlement2")]
    public enum EntitlementRequestType
    {
        /// <summary>Just a check to detremine if a user is entitled (no token generation)</summary>
        [EnumMember(Value = "check")]
        Check = 0,
    
        /// <summary>Request a DRM token</summary>
        [EnumMember(Value = "drm")]
        Drm = 1,
    
        /// <summary>Request all available CDN tokens</summary>
        [EnumMember(Value = "cdn")]
        Cdn = 2
    }
}