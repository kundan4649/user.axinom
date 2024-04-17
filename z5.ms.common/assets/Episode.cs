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
    /// Episode response object, which is returned by MS public APIs whenever a detailed info is needed on an episode
    /// </summary>
    [GeneratedCode("NJsonSchema", "9.6.5.0 (Newtonsoft.Json v9.0.0.0)")]
    public class Episode : IBusinessType
    {
        /// <summary>The unique ID</summary>
        [JsonProperty("id", Required = Required.Always)]
        [Required]
        public string Id { get; set; }
    
        /// <summary>Type of the asset (episode)</summary>
        [JsonProperty("asset_type", Required = Required.Always)]
        public AssetType AssetType { get; set; }
    
        /// <summary>Type of the episode: webisode, preview etc.</summary>
        [JsonProperty("asset_subtype", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string AssetSubtype { get; set; }

        /// <summary>Optional display name of the asset_subtype</summary>
        [JsonProperty("asset_subtype_display", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string AssetSubtypeDisplay { get; set; }
        
        /// <summary>Reference to the season this episode belongs to</summary>
        [JsonProperty("season", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public ParentReference Season { get; set; }
    
        /// <summary>Reference to the tv show this episode belongs to</summary>
        [JsonProperty("tvshow", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public ParentReference TvShow { get; set; }
    
        /// <summary>Human readable title</summary>
        [JsonProperty("title", Required = Required.Always)]
        [Required]
        [StringLength(50, MinimumLength = 1)]
        public string Title { get; set; }

        /// <summary>Original title, not translated</summary>
        [JsonProperty("original_title", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        [StringLength(50, MinimumLength = 1)]
        public string OriginalTitle { get; set; }

        /// <summary>Detailed description of the episode</summary>
        [JsonProperty("description", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string Description { get; set; }
    
        /// <summary>Short description of the episode, to be used in list views</summary>
        [JsonProperty("short_description", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string ShortDescription { get; set; }
    
        /// <summary>Index of the episode</summary>
        [JsonProperty("index", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public int? Index { get; set; }
    
        /// <summary>Age rating for parental ratings</summary>
        [JsonProperty("age_rating", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string AgeRating { get; set; }
    
        /// <summary>Duration in seconds</summary>
        [JsonProperty("duration", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public int? Duration { get; set; }
    
        /// <summary>Actors or voice actors</summary>
        [JsonProperty("actors", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public List<string> Actors { get; set; }
    
        /// <summary>Directors or Creators</summary>
        [JsonProperty("directors", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public List<string> Directors { get; set; }
    
        /// <summary>Date when this episode was first released/aired</summary>
        [JsonProperty("release_date", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(DateNotTimeConverter))]
        public DateTime? ReleaseDate { get; set; }
    
        /// <summary>Country of origin</summary>
        [JsonProperty("countries", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public List<string> Countries { get; set; }
    
        /// <summary>The rating of the item</summary>
        [JsonProperty("rating", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        [Range(0.0, 5.0)]
        public int? Rating { get; set; }
    
        /// <summary>Name of the image</summary>
        [JsonProperty("cover_image", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string CoverImage { get; set; }
    
        /// <summary>Name of the image</summary>
        [JsonProperty("list_image", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string ListImage { get; set; }
        
        /// <summary>List of assigned episode images</summary>
        [JsonProperty("image", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public JRaw Image { get; set; }
        
        /// <summary>List of genres applicable to the channel</summary>
        [JsonProperty("genres", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public List<Genre> Genres { get; set; }
    
        /// <summary>List of tags applicable to the channel</summary>
        [JsonProperty("tags", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public List<string> Tags { get; set; }
    
        /// <summary>Video stream details for the episode</summary>
        [JsonProperty("video", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public Video Video { get; set; }

        /// <summary>Trailer details</summary>
        [JsonProperty("trailer", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public Trailer Trailer { get; set; }

        /// <summary>List of extended properties (key/value pairs) for an asset. These properties are not used by MS and are simply passed through to the response models</summary>
        [JsonProperty("extended", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public JRaw Extended { get; set; }

        /// <summary>Optional business type</summary>
        [JsonProperty("business_type", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(StringEnumConverter))]
        public BusinessType? BusinessType { get; set; }

        /// <summary>The start position of the episodes intro in the format 'HH:MM:SS'</summary>
        [JsonProperty("intro_start", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string IntroStart { get; set; }
       
        /// <summary>The end position of the episodes intro in the format 'HH:MM:SS'</summary>
        [JsonProperty("intro_end", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string IntroEnd { get; set; }
       
        /// <summary>The start position of the episodes recap in the format 'HH:MM:SS'</summary>
        [JsonProperty("recap_start", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string RecapStart { get; set; }
       
        /// <summary>The end position of the episodes recap in the format 'HH:MM:SS'</summary>
        [JsonProperty("recap_end", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string RecapEnd { get; set; }

        /// <summary>The start position of the end credits in the format 'HH:MM:SS'</summary>
        [JsonProperty("end_credits_start", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string EndCreditsStart { get; set; }

        /// <summary>Related TV shows in the desired sort order.</summary>
        [JsonProperty("related_tvshows", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public List<ListItem> RelatedTvShows { get; set; }

        /// <summary>Related movies in the desired sort order.</summary>
        [JsonProperty("related_movies", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public List<ListItem> RelatedMovies { get; set; }

        /// <summary>Related collections in the desired sort order.</summary>
        [JsonProperty("related_collections", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public List<ListItem> RelatedCollections { get; set; }

        /// <summary>Related channels in the desired sort order.</summary>
        [JsonProperty("related_channels", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public List<ListItem> RelatedChannels { get; set; }
    }
}