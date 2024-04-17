using System.CodeDom.Compiler;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace z5.ms.common.assets
{
    /// <summary>The video element describes details of the video.</summary>
    [GeneratedCode("NJsonSchema", "9.6.5.0 (Newtonsoft.Json v9.0.0.0)")]
    public class Video 
    {
        /// <summary>The main URL for the video</summary>
        [JsonProperty("url", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string Url { get; set; }
    
        /// <summary>The HLS URL for this video</summary>
        [JsonProperty("hls_url", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string HlsUrl { get; set; }

        /// <summary>The DASH URL for this video</summary>
        [JsonProperty("dash_url", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string DashUrl { get; set; }

        /// <summary>The available audio track languages</summary>
        [JsonProperty("audiotracks", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public List<string> Audiotracks { get; set; }
    
        /// <summary>The available subtitle track languages</summary>
        [JsonProperty("subtitles", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public List<string> Subtitles { get; set; }
    
        /// <summary>If the video (both main URL and HLS) are DRM protected</summary>
        [JsonProperty("is_drm", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public bool? IsDrm { get; set; }
    
        /// <summary>The DRM Key ID by which the DRM license server can find the content protection key</summary>
        [JsonProperty("drm_key_id", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string DrmKeyId { get; set; }
    }
}