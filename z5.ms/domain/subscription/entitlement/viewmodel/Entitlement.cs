using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace z5.ms.domain.subscription.entitlement.viewmodel
{
    /// <summary>
    /// Query parameters for entitlement endpoint
    /// </summary>
    [Obsolete("Use entitlement2")]
    public class Entitlement 
    {
        /// <summary>The asset ID of the item that should be played back. Supported are items episodes, movies and channels (they have streams that can be played). But not TV shows, seasons, EPG programs, collections... (they do not have streams on their own).</summary>
        [JsonProperty("asset_id", Required = Required.Always)]
        [Required]
        public string AssetId { get; set; }

        /// <summary>The content key ID of the protected video stream</summary>
        [JsonProperty("key_id", Required = Required.Always)]
        [Required]
        public virtual string KeyId { get; set; }

        ///<summary>Required if the entitlement should be checked for an internal subscription/purchase. The user authentication token from the user API.</summary>
        [JsonProperty("user_token", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string UserToken { get; set; }

        /// <summary>Required if the entitlement should be checked based on a Google playstore subscription. The ID of the Google playstore subscription product.</summary>
        [JsonProperty("playstore_subscription_id", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string PlaystoreSubscriptionId { get; set; }

        /// <summary>Required if the entitlement should be checked based on a Google playstore subscription. The Google playstore subscription token of the user.</summary>
        [JsonProperty("playstore_token", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string PlaystoreToken { get; set; }

        /// <summary>Required if the entitlement should be checked based on an Apple iOS store subscription. The Apple iOS store subscription token of the user.</summary>
        [JsonProperty("ios_store_token", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string IosStoreToken { get; set; }

        /// <summary>If the entitlement message should contain the flag to make the DRM license a persistent one.</summary>
        [JsonProperty("persistent", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public bool Persistent { get; set; }
    }

    /// <summary>
    /// Query parameters for Drm required entitlement endpoint
    /// </summary>
    [Obsolete("Use entitlement2")]
    public class DrmEntitlement : Entitlement
    {
        /// <summary>The content key ID of the protected video stream</summary>
        [JsonProperty("key_id", Required = Required.Always)]
        [Required]
        public override string KeyId { get; set; }
    }
}
