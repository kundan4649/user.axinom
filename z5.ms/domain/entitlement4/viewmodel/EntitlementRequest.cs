using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace z5.ms.domain.entitlement4.viewmodel
{
    /// <summary>
    /// Request model for the entitlement API
    /// </summary>
    public class EntitlementRequest 
    {
        /// <summary>The asset ID of the item that should be played back. Supported are episodes, movies and channels (they have streams that can be played). But not TV shows, seasons, EPG programs, collections... (they do not have streams on their own).</summary>
        [JsonProperty("asset_id")]
        [Required]
        public string AssetId { get; set; }

        /// <summary>The content key ID of the protected video stream</summary>
        [JsonProperty("key_id")]
        public string KeyId { get; set; }

        /// <summary>Type of the entitlement provider:
        ///  * `internal` - uses our application logic (subscription &amp; purchase logic)
        ///  * `cms` -  MS shall trust CMS, so if CMS asks for a certain token it shall get it. Shared secret should be used to authenticate CMS
        ///  * `trailer` - always return CDN tokens to access to "{movie_id}/trailer", don't check anything. No DRM token for trailers (if asked for DRM - error).
        /// </summary>
        [JsonProperty("entitlement_provider")]
        [Required]
        public string EntitlementProvider { get; set; }
    
        // TODO: consider making this a string, it would allow to give reasonable error messages during validation instead of failing during parameter mapping with an obscure 'Object reference not set to an instance of an object'
        /// <summary>Type of the entitlement request/// </summary>
        [JsonProperty("request_type")]
        [Required]
        [JsonConverter(typeof(StringEnumConverter))]
        public EntitlementRequestType RequestType { get; set; }
    
        /// <summary>The token to use for entitlement check, it should be a user JWT token </summary>
        [JsonProperty("token")]
        public string Token { get; set; }
    
        /// <summary>If the entitlement message should contain the flag to make the DRM license a persistent one.</summary>
        [JsonProperty("persistent")]
        public bool Persistent { get; set; }

        /// <summary>The current country of the user. Required for playback of premium content with licensing restrictions.</summary>
        [JsonProperty("country")]
        [Required]
        [StringLength(2, MinimumLength = 2)]
        public string Country { get; set; }

        /// <summary>The unique device ID of the current device of the user. Required for playback of premium content.</summary>
        [JsonProperty("device_id")]
        public string DeviceId { get; set; }

        /// <summary>Additional parameters intended for inclusion in a vrl request to conviva's precision api.</summary>
        [JsonProperty("vrl_parameters")]
        public IDictionary<string, string> VrlParameters { get; set; }
    }

    /// <summary>Type of the entitlement request</summary>
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