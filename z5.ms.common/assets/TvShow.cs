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
    /// <summary>TVShow view model</summary>
    [GeneratedCode("NJsonSchema", "9.6.5.0 (Newtonsoft.Json v9.0.0.0)")] 
    public class TvShow : IBusinessType, ITvodPricing
    {
        /// <summary>The unique ID</summary>
        [JsonProperty("id", Required = Required.Always)]
        [Required]
        public string Id { get; set; }
    
        /// <summary>Type of the asset (TV show)</summary>
        [JsonProperty("asset_type", Required = Required.Always)]
        public AssetType AssetType { get; set; }
    
        /// <summary>Type of the TV show</summary>
        [JsonProperty("asset_subtype", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string AssetSubtype { get; set; }
    
        /// <summary>Human readable title</summary>
        [JsonProperty("title", Required = Required.Always)]
        [Required]
        [StringLength(50, MinimumLength = 1)]
        public string Title { get; set; }

        /// <summary>Original title, not translated</summary>
        [JsonProperty("original_title", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        [StringLength(50, MinimumLength = 1)]
        public string OriginalTitle { get; set; }

        /// <summary>Detailed description of the TV show</summary>
        [JsonProperty("description", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string Description { get; set; }
    
        /// <summary>Short description of the TV show, to be used in list views</summary>
        [JsonProperty("short_description", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string ShortDescription { get; set; }
    
        /// <summary>Date when this TV show was first released/aired</summary>
        [JsonProperty("release_date", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(DateNotTimeConverter))]
        public DateTime? ReleaseDate { get; set; }
    
        /// <summary>Age rating for parental ratings</summary>
        [JsonProperty("age_rating", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string AgeRating { get; set; }
    
        /// <summary>List of genres applicable to the TV show</summary>
        [JsonProperty("genres", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public List<Genre> Genres { get; set; }
    
        /// <summary>List of tags applicable to the TV show</summary>
        [JsonProperty("tags", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public List<string> Tags { get; set; }
    
        /// <summary>Actors or voice actors</summary>
        [JsonProperty("actors", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public List<string> Actors { get; set; }
    
        /// <summary>Directors or Creators</summary>
        [JsonProperty("directors", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public List<string> Directors { get; set; }
    
        /// <summary>Country of origin</summary>
        [JsonProperty("countries", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public List<string> Countries { get; set; }
    
        /// <summary>The rating of the item</summary>
        [JsonProperty("rating", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        [Range(0.0, 5.0)]
        public int? Rating { get; set; }
    
        /// <summary>Available audio languages in the episodes</summary>
        [JsonProperty("languages", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public List<string> Languages { get; set; }
    
        /// <summary>Available subtitle languages in the episodes</summary>
        [JsonProperty("subtitle_languages", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public List<string> SubtitleLanguages { get; set; }
    
        /// <summary>Name of the image</summary>
        [JsonProperty("cover_image", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string CoverImage { get; set; }
    
        /// <summary>Name of the image</summary>
        [JsonProperty("list_image", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string ListImage { get; set; }
        
        /// <summary>List of assigned TV show images</summary>
        [JsonProperty("image", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public JRaw Image { get; set; }
    
        /// <summary>Licensing details for the TV show</summary>    
        [JsonProperty("licensing", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public Licensing Licensing { get; set; }
    
        /// <summary>List of channel IDs in which the tv show is aired.</summary>
        [JsonProperty("channels", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public List<ListItem> Channels { get; set; }
    
        /// <summary>The business type of the TV show: free, ad, premium etc.</summary>
        [JsonProperty("business_type", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(StringEnumConverter))]
        public BusinessType? BusinessType { get; set; }

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

        /// <summary>List of the TV show's seasons, with reduced details to be used in a list view</summary>
        [JsonProperty("seasons", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public List<ListItem> Seasons { get; set; }

        /// <summary>Trailer details</summary>
        [JsonProperty("trailer", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public Trailer Trailer { get; set; }

        /// <summary>List of extended properties (key/value pairs) for an asset. These properties are not used by MS and are simply passed through to the response models</summary>
        [JsonProperty("extended", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public JRaw Extended { get; set; }

        /// <summary>Tvod pricing information</summary>
        [JsonProperty("tvod", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public TvodPricing TvodPricing { get; set; }

        /// <summary>Parental restriction</summary>
        [JsonProperty("parental_restriction", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public int? ParentalRestriction { get; set; }

        /// <summary>Warning message</summary>
        [JsonProperty("warning_message", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public bool? WarningMessage { get; set; }
    }
}