using System;
using System.CodeDom.Compiler;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using z5.ms.common.assets.common;

namespace z5.ms.common.assets
{
    /// <summary>
    /// Subset of EPG program response object for reminders
    /// </summary>
    [GeneratedCode("NJsonSchema", "9.6.5.0 (Newtonsoft.Json v9.0.0.0)")]
    public class EpgProgramReminder 
    {
        /// <summary>The unique ID</summary>
        [JsonProperty("id", Required = Required.Always)]
        [Required]
        public string Id { get; set; }
    
        /// <summary>Reference to parent channel</summary>
        [JsonProperty("channel", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public ParentReference Channel { get; set; }

        /// <summary>Reference to parent channel</summary>
        [JsonProperty("channel_name", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string ChannelName { get; set; }

        /// <summary>The unique ID of the related VOD item. This can be a movie or an episode - depending on the 'vod_asset_type' (or none if none was matched).</summary>
        [JsonProperty("vod_id", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string VodId { get; set; }
    
        /// <summary>Asset type of the VOD item</summary>
        [JsonProperty("vod_asset_type", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public AssetType? VodAssetType { get; set; }
    
        /// <summary>The movie or TV show title (not the episode title)</summary>
        [JsonProperty("title", Required = Required.Always)]
        [Required]
        [StringLength(50, MinimumLength = 1)]
        public string Title { get; set; }
    
        /// <summary>Duration in seconds</summary>
        [JsonProperty("duration", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public int? Duration { get; set; }

        /// <summary>Starting time of the program in UTC time</summary>
        [JsonProperty("start_time", Required = Required.Always)]
        [Required]
        public DateTime StartTime { get; set; }
    
        /// <summary>Ending time of the program in UTC time</summary>
        [JsonProperty("end_time", Required = Required.Always)]
        [Required]
        public DateTime EndTime { get; set; }
    }
}