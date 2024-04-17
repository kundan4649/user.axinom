using System;
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
    /// Live event response object, which is returned by MS public APIs whenever a detailed info is needed on a live event 
    /// </summary>
    [GeneratedCode("NJsonSchema", "9.6.5.0 (Newtonsoft.Json v9.0.0.0)")]
    public class LiveEvent : IBusinessType
    {
        /// <summary>The unique ID</summary>
        [JsonProperty("id", Required = Required.Always)]
        [Required]
        public string Id { get; set; }
    
        /// <summary>Type of the asset (live event)</summary>
        [JsonProperty("asset_type", Required = Required.Always)]
        public AssetType AssetType { get; set; }
    
        /// <summary>Human readable title</summary>
        [JsonProperty("title", Required = Required.Always)]
        [Required]
        [StringLength(50, MinimumLength = 1)]
        public string Title { get; set; }
    
        /// <summary>Detailed description of the live event</summary>
        [JsonProperty("description", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string Description { get; set; }
    
        /// <summary>Short description of the live event, to be used in list views</summary>
        [JsonProperty("short_description", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string ShortDescription { get; set; }
    
        /// <summary>The live streaming URL for HLS playback</summary>
        [JsonProperty("stream_url_hls", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string StreamUrlHls { get; set; }
    
        /// <summary>The live streaming URL for MPEG DASH playback</summary>
        [JsonProperty("stream_url", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string StreamUrl { get; set; }
    
        /// <summary>List of tags applicable to the live event</summary>
        [JsonProperty("tags", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public List<string> Tags { get; set; }
    
        /// <summary>Available audio languages for the channel</summary>
        [JsonProperty("languages", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public List<string> Languages { get; set; }
    
        /// <summary>Available subtitle languages for the channel</summary>
        [JsonProperty("subtitle_languages", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public List<string> SubtitleLanguages { get; set; }
    
        /// <summary>Name of the image</summary>
        [JsonProperty("cover_image", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string CoverImage { get; set; }
    
        /// <summary>Name of the image</summary>
        [JsonProperty("list_image", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string ListImage { get; set; }
    
        /// <summary>List of assigned live event images</summary>
        [JsonProperty("image", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public JRaw Image { get; set; }
        
        /// <summary>The business type of the live event: free, ad, premium etc.</summary>
        [JsonProperty("business_type", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(StringEnumConverter))]
        public BusinessType? BusinessType { get; set; }

        /// <summary>Scheduled starting time of the live event (UTC)</summary>
        [JsonProperty("scheduled_start", Required = Required.Always)]
        [Required]
        public DateTime ScheduledStart { get; set; }

        /// <summary>Scheduled ending time of the live event (UTC)</summary>
        [JsonProperty("scheduled_end", Required = Required.Always)]
        [Required]
        public DateTime ScheduledEnd { get; set; }
    }
}