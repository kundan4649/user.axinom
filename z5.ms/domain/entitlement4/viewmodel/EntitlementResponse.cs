using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using z5.ms.common.assets.common;

namespace z5.ms.domain.entitlement4.viewmodel
{
    /// <summary>
    /// Response to the entitlement query
    /// </summary>
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

        /// <summary>Asset business type, e.g. Free, Ad</summary>
        /// <remarks>Items with business_type=Ad require client app to display ad to the customer before wathing the movie</remarks>
        [JsonProperty("business_type", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(StringEnumConverter))]
        public BusinessType? BusinessType { get; set; }

        /// <summary>If the entitlement service can issue a entitlement message that allows the creation of a persistent DRM license</summary>
        [JsonProperty("is_downloadable", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public bool? IsDownloadable { get; set; }

        /// <summary>Number of hours after live broadcast that channel playback is supported. (Not checked by entitlement)</summary>
        [JsonProperty("catch_up_hours", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public int? CatchUpHours { get; set; }

        /// <summary>The asset ID of the subscription plan that was selected.</summary>
        [JsonProperty("subscription_plan_id", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string SubscriptionPlanId { get; set; }

        /// <summary>A prioritized list of absolute video urls.</summary>
        [JsonProperty("video", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public List<Video> Videos { get; set; }
    }

    /// <summary>Absolute urls for a video file/stream.</summary>
    public class Video
    {
        /// <summary>The absolute main URL for this video</summary>
        [JsonProperty("url", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string Url { get; set; }

        /// <summary>An absolute HLS URL for this video</summary>
        [JsonProperty("hls_url", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string HlsUrl { get; set; }
    }
}