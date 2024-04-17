using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using z5.ms.common.assets.common;

namespace z5.ms.common.assets
{
    /// <summary>Basic channel details containing EPG program items of this channel. Sorted by the start date/time ascending.</summary>
    [GeneratedCode("NJsonSchema", "9.6.5.0 (Newtonsoft.Json v9.0.0.0)")]
    public class EpgChannel 
    {
        /// <summary>The unique ID of the channel.</summary>
        [JsonProperty("id", Required = Required.Always)]
        [Required]
        public string Id { get; set; }
        
        /// <summary>Type the an asset</summary>
        [JsonProperty("asset_type", Required = Required.Always)]
        public AssetType AssetType { get; set; }
    
        /// <summary>Human readable title for this channel</summary>
        [JsonProperty("title", Required = Required.Always)]
        [Required]
        [StringLength(50, MinimumLength = 1)]
        public string Title { get; set; }

        /// <summary>Original title, not translated</summary>
        [JsonProperty("original_title", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        [StringLength(50, MinimumLength = 1)]
        public string OriginalTitle { get; set; }

        /// <summary>List of genres applicable to the channel</summary>
        [JsonProperty("genres", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public List<Genre> Genres { get; set; }
    
        /// <summary>Name of the image.</summary>
        [JsonProperty("list_image", Required = Required.Default)]
        [StringLength(int.MaxValue)]
        public string ListImage { get; set; }
        
        /// <summary>Name of the cover image</summary>
        [JsonProperty("cover_image", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string CoverImage { get; set; }
        
        /// <summary>List of assigned EPG channel images</summary>
        [JsonProperty("image", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public JRaw Image { get; set; }
    
        /// <summary>EPG program data for this channel</summary>
        [JsonProperty("items", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public List<EpgProgram> Items { get; set; }
    }
}