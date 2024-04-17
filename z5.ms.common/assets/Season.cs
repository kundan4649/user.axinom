using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using z5.ms.common.assets.common;

namespace z5.ms.common.assets
{
    /// <summary>
    /// Season response object, which is returned by MS public APIs whenever a detailed info is needed on a season  
    /// </summary>
    [GeneratedCode("NJsonSchema", "9.6.5.0 (Newtonsoft.Json v9.0.0.0)")]
    public class Season 
    {
        /// <summary>The unique ID</summary>
        [JsonProperty("id", Required = Required.Always)]
        [Required]
        public string Id { get; set; }
    
        /// <summary>Type of the asset (season)</summary>
        [JsonProperty("asset_type", Required = Required.Always)]
        public AssetType AssetType { get; set; }
    
        /// <summary>Reference to the tv show this season belongs to</summary>
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

        /// <summary>Detailed description of the season</summary>
        [JsonProperty("description", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string Description { get; set; }
    
        /// <summary>Short description of the season, to be used in list views</summary>
        [JsonProperty("short_description", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string ShortDescription { get; set; }
    
        /// <summary>Index of the season within the TV show</summary>
        [JsonProperty("index", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public int? Index { get; set; }
    
        /// <summary>Name of the image</summary>
        [JsonProperty("list_image", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string ListImage { get; set; }
        
        /// <summary>Name of the cover image</summary>
        [JsonProperty("cover_image", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string CoverImage { get; set; }
        
        /// <summary>List of assigned season images</summary>
        [JsonProperty("image", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public JRaw Image { get; set; }

        /// <summary>Trailer details</summary>
        [JsonProperty("trailer", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public Trailer Trailer { get; set; }

        /// <summary>The total number of episodes in this season.</summary>
        [JsonProperty("total", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public int? Total { get; set; }
    
        /// <summary>The page of the result set (one based).</summary>
        [JsonProperty("page", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        [Range(1.0, double.MaxValue)]
        public int? Page { get; set; }
    
        /// <summary>Max amount of episodes returned per page.</summary>
        [JsonProperty("page_size", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        [Range(1.0, 100.0)]
        public int? PageSize { get; set; }
    
        /// <summary>Episodes in descending sort order based on episode index</summary>
        [JsonProperty("episodes", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public List<Episode> Episodes { get; set; }
 
        /// <summary>If the viewer can skip for episodes in this season the intro or recap</summary>
        [JsonProperty("skip_available", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public bool SkipAvailable { get; set; }

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
    }
}