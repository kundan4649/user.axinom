using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using z5.ms.common.assets.common;

namespace z5.ms.common.assets
{
    /// <summary>
    /// Live channel response object, which is returned by MS public APIs whenever a detailed info is needed on a live channel
    /// </summary>
    [GeneratedCode("NJsonSchema", "9.6.5.0 (Newtonsoft.Json v9.0.0.0)")]
    public class Channel : IBusinessType, ITvodPricing
    {
        /// <summary>The unique ID</summary>
        [JsonProperty("id", Required = Required.Always)]
        [Required]
        public string Id { get; set; }

        /// <summary>Type of the asset (live channel)</summary>
        [JsonProperty("asset_type", Required = Required.Always)]
        public AssetType AssetType { get; set; }

        /// <summary>Human readable title</summary>
        [JsonProperty("title", Required = Required.Always)]
        [Required]
        [StringLength(50, MinimumLength = 1)]
        public string Title { get; set; }

        /// <summary>Original title, not translated</summary>
        [JsonProperty("original_title", NullValueHandling = NullValueHandling.Ignore, Required = Required.Default)]
        [StringLength(50, MinimumLength = 1)]
        public string OriginalTitle { get; set; }

        /// <summary>Detailed description of the channel</summary>
        [JsonProperty("description", NullValueHandling = NullValueHandling.Ignore, Required = Required.Default)]
        public string Description { get; set; }

        /// <summary>Short description of the channel, to be used in list views</summary>
        [JsonProperty("short_description", NullValueHandling = NullValueHandling.Ignore, Required = Required.Default)]
        public string ShortDescription { get; set; }

        /// <summary>The live streaming URL for HLS playback</summary>
        [JsonProperty("stream_url_hls", NullValueHandling = NullValueHandling.Ignore, Required = Required.Default)]
        public string StreamUrlHls { get; set; }

        /// <summary>The live streaming URL for MPEG DASH playback</summary>
        [JsonProperty("stream_url", NullValueHandling = NullValueHandling.Ignore, Required = Required.Default)]
        public string StreamUrl { get; set; }

        /// <summary>The URL to use for catch up streaming with HLS playback. Replace '{0}' with the unix time stamp for start and '{1}' with the desired duration in seconds (for endless playback use 'null').</summary>
        [JsonProperty("catch_up_url_hls", NullValueHandling = NullValueHandling.Ignore, Required = Required.Default)]
        public string CatchUpUrlHls { get; set; }

        /// <summary>The URL to use for catch up streaming with MPEG DASH playback. Replace '{0}' with the unix time stamp for start and '{1}' with the desired duration in seconds (for endless playback use 'null').</summary>
        [JsonProperty("catch_up_url", NullValueHandling = NullValueHandling.Ignore, Required = Required.Default)]
        public string CatchUpUrl { get; set; }

        /// <summary>The number of hours for which catch up playback is available in this channel.</summary>
        [JsonProperty("catch_up_hours", NullValueHandling = NullValueHandling.Ignore, Required = Required.Default)]
        public string CatchUpHours { get; set; }

        /// <summary>The offset from UTC-0 e.g. +01:00 for suggestion to adjust the display time in the UI.</summary>
        [JsonProperty("time_offset", NullValueHandling = NullValueHandling.Ignore, Required = Required.Default)]
        public string TimeOffset { get; set; }

        /// <summary>If the video (both main URL and HLS) are DRM protected</summary>
        [JsonProperty("is_drm", NullValueHandling = NullValueHandling.Ignore, Required = Required.Default)]
        public bool IsDrm { get; set; }

        /// <summary>The DRM Key ID by which the DRM license server can find the content protection key</summary>
        [JsonProperty("drm_key_id", NullValueHandling = NullValueHandling.Ignore, Required = Required.Default)]
        public string DrmKeyId { get; set; }

        /// <summary>List of genres applicable to the channel</summary>
        [JsonProperty("genres", NullValueHandling = NullValueHandling.Ignore, Required = Required.Default)]
        public List<Genre> Genres { get; set; }

        /// <summary>List of tags applicable to the channel</summary>
        [JsonProperty("tags", NullValueHandling = NullValueHandling.Ignore, Required = Required.Default)]
        public List<string> Tags { get; set; }

        /// <summary>Available audio languages for the channel</summary>
        [JsonProperty("languages", NullValueHandling = NullValueHandling.Ignore, Required = Required.Default)]
        public List<string> Languages { get; set; }

        /// <summary>Available subtitle languages for the channel</summary>
        [JsonProperty("subtitle_languages", NullValueHandling = NullValueHandling.Ignore, Required = Required.Default)]
        public List<string> SubtitleLanguages { get; set; }

        /// <summary>Name of the image</summary>
        [JsonProperty("cover_image", NullValueHandling = NullValueHandling.Ignore, Required = Required.Default)]
        public string CoverImage { get; set; }

        /// <summary>Name of the image</summary>
        [JsonProperty("list_image", NullValueHandling = NullValueHandling.Ignore, Required = Required.Default)]
        public string ListImage { get; set; }

        /// <summary>List of assigned channel images</summary>
        [JsonProperty("image", NullValueHandling = NullValueHandling.Ignore, Required = Required.Default)]
        public JRaw Image { get; set; }

        /// <summary>Related TV shows in the desired sort order.</summary>
        [JsonProperty("related", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public List<ListItem> Related { get; set; }

        /// <summary>Related movies in the desired sort order.</summary>
        [JsonProperty("related_movies", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public List<ListItem> RelatedMovies { get; set; }

        /// <summary>Related collections in the desired sort order.</summary>
        [JsonProperty("related_collections", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public List<ListItem> RelatedCollections { get; set; }

        /// <summary>Related channels in the desired sort order.</summary>
        [JsonProperty("related_channels", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public List<ListItem> RelatedChannels { get; set; }

        /// <summary>Licensing details for the program</summary>
        [JsonProperty("licensing", NullValueHandling = NullValueHandling.Ignore, Required = Required.Default)]
        public Licensing Licensing { get; set; }

        /// <summary>The business type of the channel: free, ad, premium etc.</summary>
        [JsonProperty("business_type", NullValueHandling = NullValueHandling.Ignore, Required = Required.Default)]
        [JsonConverter(typeof(StringEnumConverter))]
        public BusinessType? BusinessType { get; set; }

        /// <summary>Tvod pricing information</summary>
        [JsonProperty("tvod", NullValueHandling = NullValueHandling.Ignore, Required = Required.Default)]
        public TvodPricing TvodPricing { get; set; }
        
        /// <summary>List of extended properties (key/value pairs) for an asset. These properties are not used by MS and are simply passed through to the response models</summary>
        [JsonProperty("extended", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public JRaw Extended { get; set; }
    }
}