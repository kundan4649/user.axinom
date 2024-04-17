using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using z5.ms.common.assets.common;
using z5.ms.common.helpers;

namespace z5.ms.common.assets
{
    /// <summary>
    /// View/response model for an audio album
    /// </summary>
    [GeneratedCode("NJsonSchema", "9.10.10.0 (Newtonsoft.Json v9.0.0.0)")]
    public class Album : IBusinessType
    {
        /// <summary>The unique ID</summary>
        [JsonProperty("id", Required = Required.Always)]
        [Required]
        public string Id { get; set; }
    
        /// <summary>Asset type</summary>
        [JsonProperty("asset_type", Required = Required.Always)]
        public AssetType AssetType { get; set; }
    
        /// <summary>Optional asset subtype</summary>
        [JsonProperty("asset_subtype", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string AssetSubtype { get; set; }

        /// <summary>Human readable title</summary>
        [JsonProperty("title", Required = Required.Always)]
        [Required]
        [StringLength(50, MinimumLength = 1)]
        public string Title { get; set; }
   
        /// <summary>Release date of the album</summary>
        [JsonProperty("release_date", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(DateNotTimeConverter))]
        public DateTime? ReleaseDate { get; set; }
    
        /// <summary>Album description</summary>
        [JsonProperty("description", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string Description { get; set; }
    
        /// <summary>Actors or voice actors</summary>
        [JsonProperty("artists", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public List<string> Artists { get; set; }
    
        /// <summary>List of assigned album images</summary>
        [JsonProperty("image", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public JRaw Image { get; set; }
    
        /// <summary>Album genres</summary>
        [JsonProperty("genres", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public List<Genre> Genres { get; set; }
    
        /// <summary>Album tags</summary>
        [JsonProperty("tags", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public List<string> Tags { get; set; }
        
        /// <summary>Optional business type</summary>
        [JsonProperty("business_type", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(StringEnumConverter))]
        public BusinessType? BusinessType { get; set; }
        
        /// <summary>Movie lisensing</summary>
        [JsonProperty("licensing", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public Licensing Licensing { get; set; }
    
        /// <summary>audio tracks</summary>
        [JsonProperty("tracks", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public List<Track> Tracks { get; set; }

        /// <summary>Parental restriction</summary>
        [JsonProperty("parental_restriction", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public int? ParentalRestriction { get; set; }

        /// <summary>List of extended properties (key/value pairs) for an asset. These properties are not used by MS and are simply passed through to the response models</summary>
        [JsonProperty("extended", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public JRaw Extended { get; set; }
    }
}