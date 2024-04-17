using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace z5.ms.domain.entitlement4.viewmodel
{
    /// <summary>
    /// Query parameters for entitlement endpoint
    /// </summary>
    public class Entitlement 
    {
        /// <summary>The asset ID of the item that should be played back. Supported are items episodes, movies and channels (they have streams that can be played). But not TV shows, seasons, EPG programs, collections... (they do not have streams on their own).</summary>
        [JsonProperty("asset_id", Required = Required.Always)]
        [Required]
        public string AssetId { get; set; }

        /// <summary>The content key ID of the protected video stream</summary>
        [JsonProperty("key_id", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public virtual string KeyId { get; set; }

        ///<summary>Required if the entitlement should be checked for an internal subscription/purchase. The user authentication token from the user API.</summary>
        [JsonProperty("token", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string UserToken { get; set; }

        /// <summary>If the entitlement message should contain the flag to make the DRM license a persistent one.</summary>
        [JsonProperty("persistent", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public bool Persistent { get; set; }

        /// <summary>The current country of the user. Required for playback of premium content with licensing restrictions.</summary>
        [JsonProperty("country", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string Country { get; set; }

        /// <summary>The unique device ID of the current device of the user. Required for playback of premium content.</summary>
        [JsonProperty("device_id", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string DeviceId { get; set; }
    }

    /// <summary>
    /// Query parameters for Drm required entitlement endpoint
    /// </summary>
    public class DrmEntitlement : Entitlement
    {
        /// <summary>The content key ID of the protected video stream</summary>
        [JsonProperty("key_id", Required = Required.Always)]
        [Required]
        public override string KeyId { get; set; }
    }
}
