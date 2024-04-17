using Newtonsoft.Json;

namespace z5.ms.domain.entitlement4.viewmodel
{
    /// <summary>Collection entitlements list base element. </summary>
    public class CollectionEntitlementItem
    {
        /// <summary>Collection item's Id </summary>
        [JsonProperty("asset_id", Required = Required.Always)]
        public string AssetId { get; set; }

        /// <summary> Collection item's Url of Manifest.mpd file </summary>
        [JsonProperty("mpd_url", Required = Required.AllowNull, NullValueHandling = NullValueHandling.Include)] 
        public string MpdUrl { get; set; }

        /// <summary> Collection item's Drm protection is disabled</summary>
        [JsonProperty("drm_disabled", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public bool? DrmDisabled { get; set; }
        
        /// <summary> Collection item's entitlement response </summary>
        [JsonProperty("entitlement_response")]
        public EntitlementResponse EntitlementResponse { get; set; }
        
        /// <summary>Entitlement list element constructor </summary>
        /// <param name="assetId">Element's asset id. </param>
        public CollectionEntitlementItem(string assetId)
        {
            AssetId = assetId;
        }
    }
}