using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

namespace z5.ms.common.assets.common
{
    /// <summary>List item view model</summary>
    [GeneratedCode("NJsonSchema", "9.6.5.0 (Newtonsoft.Json v9.0.0.0)")]
    public class ListItem : IBusinessType
    {
        /// <summary>The unique ID of the item.</summary>
        [JsonProperty("id", Required = Required.Always)]
        [Required]
        public string Id { get; set; }

        /// <summary>Unique asset type</summary>
        [JsonProperty("asset_type", Required = Required.Always)]
        public AssetType AssetType { get; set; }

        /// <summary>Optional asset subtype</summary>
        [JsonProperty("asset_subtype", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string AssetSubtype { get; set; }

        /// <summary>Human readable title for this item</summary>
        [JsonProperty("title", Required = Required.Default)]
        [StringLength(50)]
        public string Title { get; set; }
        
        /// <summary>Alternative title used to generate search engine optimised page titles</summary>
        [JsonProperty("seo_title", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string SeoTitle { get; set; }

        /// <summary>Original title, not translated</summary>
        [JsonProperty("original_title", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        [StringLength(50, MinimumLength = 1)]
        public string OriginalTitle { get; set; }

        /// <summary>Asset's description. Shall only be present if asset_type is a Collection and if the field is not empty</summary>
        [JsonProperty("description", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string Description { get; set; }

        /// <summary>Asset business type</summary>
        [JsonProperty("business_type", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(StringEnumConverter))]
        public BusinessType? BusinessType { get; set; }

        /// <summary>The playback duration of this item in seconds if available.</summary>
        [JsonProperty("duration", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public int? Duration { get; set; }

        /// <summary>The genres of this item if available</summary>
        [JsonProperty("genres", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public List<Genre> Genres { get; set; }

        /// <summary>Tags list</summary>
        [JsonProperty("tags", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public List<string> Tags { get; set; }

        /// <summary>Name of the image.</summary>
        [JsonProperty("list_image", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        [StringLength(int.MaxValue)]
        public string ListImage { get; set; }

        /// <summary>Name of the cover image</summary>
        [JsonProperty("cover_image", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string CoverImage { get; set; }
        
        /// <summary>List of assigned asset images</summary>
        [JsonProperty("image", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public JRaw Image { get; set; }
        
        /// <summary>Available languages. For TV shows, movies, channels and collections in the collection API endpoint only.</summary>
        [JsonProperty("languages", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public List<string> Languages { get; set; }
        
        /// <summary>Available subtitle languages. For TV shows, movies and channels in the collection API endpoint only.</summary>
        [JsonProperty("subtitle_languages", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public List<string> SubtitleLanguages { get; set; }
        
        /// <summary>The countries if the item is a collection (only exposed in the collection API endpoint).</summary>
        [JsonProperty("countries", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public List<string> Countries { get; set; }

        /// <summary>The start date and time (for EPG items).</summary>
        [JsonProperty("start_time", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? StartTime { get; set; }

        /// <summary>The end date and time (for EPG items).</summary>
        [JsonProperty("end_time", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? EndTime { get; set; }

        /// <summary>Only collection type list items can contain nested "list_item" in this array</summary>
        [JsonProperty("items", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public List<ListItem> Items { get; set; }

        /// <summary>A price tier at which the asset should be available in this country</summary>
        [JsonProperty("tvod_tier", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string TvodTier { get; set; }

        /// <summary>Parental restriction</summary>
        [JsonProperty("parental_restriction", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public int? ParentalRestriction { get; set; }

        /// <summary>Warning message</summary>
        [JsonProperty("warning_message", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public bool? WarningMessage { get; set; }
        
        /// <summary>Collection Auto Play</summary>
        [JsonProperty("collection_auto_play", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public bool? CollectionAutoPlay { get; set; }
        
        /// <summary>URL name for SEO optimization</summary>
        [JsonProperty("url_name", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string UrlName { get; set; }
    }
}