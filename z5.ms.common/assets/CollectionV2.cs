using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using z5.ms.common.assets.common;

namespace z5.ms.common.assets
{
    /// <summary>Collection view model</summary>
    public class CollectionV2 
    {
        /// <summary>The unique ID</summary>
        [JsonProperty("id", Required = Required.Always)]
        [Required]
        public string Id { get; set; }
    
        /// <summary>Unique asset type</summary>
        [JsonProperty("asset_type", Required = Required.Always)]
        public AssetType AssetType { get; set; }
    
        /// <summary>Human readable title</summary>
        [JsonProperty("title", Required = Required.Always)]
        [Required]
        [StringLength(50, MinimumLength = 1)]
        public string Title { get; set; }

        /// <summary>Original title, not translated</summary>
        [JsonProperty("original_title", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        [StringLength(50, MinimumLength = 1)]
        public string OriginalTitle { get; set; }

        /// <summary>Description</summary>
        [JsonProperty("description", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string Description { get; set; }
    
        /// <summary>Short descrtiption</summary>
        [JsonProperty("short_description", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string ShortDescription { get; set; }
    
        /// <summary>Name of the image</summary>
        [JsonProperty("cover_image", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string CoverImage { get; set; }
    
        /// <summary>Name of the image</summary>
        [JsonProperty("list_image", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string ListImage { get; set; }
        
        /// <summary>List of assigned collection images</summary>
        [JsonProperty("image", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public JRaw Image { get; set; }

        /// <summary>Tags list</summary>
        [JsonProperty("tags", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public List<string> Tags { get; set; }
    
        /// <summary>The total number of items found by the query.</summary>
        [JsonProperty("total", Required = Required.Always)]
        public int Total { get; set; }
    
        /// <summary>The page of the result set (one based).</summary>
        [JsonProperty("page", Required = Required.Always)]
        [Range(1.0, double.MaxValue)]
        public int Page { get; set; }
    
        /// <summary>Max amount of items returned per page.</summary>
        [JsonProperty("page_size", Required = Required.Always)]
        [Range(1.0, 100.0)]
        public int PageSize { get; set; }
        
        /// <summary>The languages for this collection.</summary>
        [JsonProperty("languages", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public List<string> Languages { get; set; }
        
        /// <summary>The countries for this collection.</summary>
        [JsonProperty("countries", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public List<string> Countries { get; set; }
    
        /// <summary>The items from the collection for the current paged result set</summary>
        [JsonProperty("items", Required = Required.Always)]
        [Required]
        public List<object> Items { get; set; } = new List<object>();

        /// <summary>List of extended properties (key/value pairs) for an asset. These properties are not used by MS and are simply passed through to the response models</summary>
        [JsonProperty("extended", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public JRaw Extended { get; set; }
    }
}