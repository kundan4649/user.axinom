using System.CodeDom.Compiler;
using Newtonsoft.Json;

namespace z5.ms.common.assets
{
    /// <summary>The video element describes details of the video.</summary>
    [GeneratedCode("NJsonSchema", "9.6.5.0 (Newtonsoft.Json v9.0.0.0)")]
    public class Trailer 
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
    }
}