using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using z5.ms.common.assets.common;

namespace z5.ms.common.assets
{
    /// <summary>
    /// EPG program response object, which is returned by MS public APIs whenever a detailed info is needed on an EPG program  
    /// </summary>
    [GeneratedCode("NJsonSchema", "9.6.5.0 (Newtonsoft.Json v9.0.0.0)")]
    public class EpgProgram 
    {
        /// <summary>The unique ID</summary>
        [JsonProperty("id", Required = Required.Always)]
        [Required]
        public string Id { get; set; }
    
        /// <summary>Type of the asset (EPG program)</summary>
        [JsonProperty("asset_type", Required = Required.Always)]
        public AssetType AssetType { get; set; }
    
        /// <summary>Reference to parent channel</summary>
        [JsonProperty("channel", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public ParentReference Channel { get; set; }
    
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

        /// <summary>Original title, not translated</summary>
        [JsonProperty("original_title", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        [StringLength(50, MinimumLength = 1)]
        public string OriginalTitle { get; set; }

        /// <summary>A short description of the program</summary>
        [JsonProperty("description", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string Description { get; set; }
    
        /// <summary>The movie or episode description (not the TV show description)</summary>
        [JsonProperty("short_description", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string ShortDescription { get; set; }
    
        /// <summary>Duration in seconds</summary>
        [JsonProperty("duration", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public int? Duration { get; set; }
    
        /// <summary>Actors or voice actors</summary>
        [JsonProperty("actors", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public List<string> Actors { get; set; }
    
        /// <summary>Starting time of the program in UTC time</summary>
        [JsonProperty("start_time", Required = Required.Always)]
        [Required]
        public DateTime StartTime { get; set; }
    
        /// <summary>Ending time of the program in UTC time</summary>
        [JsonProperty("end_time", Required = Required.Always)]
        [Required]
        public DateTime EndTime { get; set; }
    
        /// <summary>Name of the image</summary>
        [JsonProperty("cover_image", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string CoverImage { get; set; }
    
        /// <summary>Name of the image</summary>
        [JsonProperty("list_image", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string ListImage { get; set; }
        
        /// <summary>List of assigned EPG program images</summary>
        [JsonProperty("image", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public JRaw Image { get; set; }
    
        /// <summary>List of genres applicable to the program</summary>
        [JsonProperty("genres", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public List<Genre> Genres { get; set; }
    
        /// <summary>List of tags applicable to the program</summary>
        [JsonProperty("tags", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public List<string> Tags { get; set; }
    
        /// <summary>Licensing details for the program</summary>
        [JsonProperty("licensing", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public Licensing Licensing { get; set; }
    }
}