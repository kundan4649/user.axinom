using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using z5.ms.common.assets.common;
using z5.ms.common.helpers;

namespace z5.ms.common.assets
{
    /// <summary>A File Asset</summary>
    public class File
    {
        /// <summary>The unique ID</summary>
        [JsonProperty("id", Required = Required.Always)]
        [Required]
        public string Id { get; set; }
    
        /// <summary>Unique asset type</summary>
        [JsonProperty("asset_type", Required = Required.Always)]
        [Required]
        public AssetType AssetType { get; set; }
    
        /// <summary>Optional asset subtype</summary>
        [JsonProperty("asset_subtype", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string AssetSubtype { get; set; }
    
        /// <summary>Human readable title</summary>
        [JsonProperty("title", Required = Required.Always)]
        [Required]
        public string Title { get; set; }

        /// <summary>Original title, not translated</summary>
        [JsonProperty("original_title", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string OriginalTitle { get; set; }

        /// <summary>Movie description</summary>
        [JsonProperty("description", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string Description { get; set; }
    
        /// <summary>Movie short descripttion</summary>
        [JsonProperty("short_description", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string ShortDescription { get; set; }
        
        /// <summary>Age rating for parental ratings</summary>
        [JsonProperty("age_rating", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string AgeRating { get; set; }
    
        /// <summary>Country of origin</summary>
        [JsonProperty("countries", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public List<string> Countries { get; set; }
        
        /// <summary>Assigned file image</summary>
        [JsonProperty("image", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public JRaw Image { get; set; }
    
        /// <summary>File tags</summary>
        [JsonProperty("tags", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public List<string> Tags { get; set; }
    
        /// <summary>Movie lisensing</summary>
        [JsonProperty("licensing", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public Licensing Licensing { get; set; }
        
        /// <summary>Release date</summary>
        [JsonProperty("released", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(DateNotTimeConverter))]
        public DateTime? ReleaseDate { get; set; }
        
        /// <summary>Optional business type</summary>
        [JsonProperty("business_type", Required = Required.Always, NullValueHandling = NullValueHandling.Ignore)]
        [Required]
        [JsonConverter(typeof(StringEnumConverter))]
        public BusinessType? BusinessType { get; set; }
        
        /// <summary>Relative asset URL</summary>
        [JsonProperty("url", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string Url { get; set; }
    }
}