using System.CodeDom.Compiler;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using z5.ms.common.assets.common;

namespace z5.ms.common.assets
{
    /// <summary>A notification object to show some message to the user</summary>
    [GeneratedCode("NJsonSchema", "9.6.5.0 (Newtonsoft.Json v9.0.0.0)")]
    public class Notification 
    {
        /// <summary>The unique ID that should be remembered in the user profile to display the notification only once.</summary>
        [JsonProperty("id", Required = Required.Always)]
        [Required]
        public string Id { get; set; }
    
        /// <summary>Type of the asset (notification)</summary>
        [JsonProperty("asset_type", Required = Required.Always)]
        public AssetType AssetType { get; set; }
    
        /// <summary>The title of the notification</summary>
        [JsonProperty("title", Required = Required.Always)]
        [Required]
        [StringLength(50, MinimumLength = 1)]
        public string Title { get; set; }
    
        /// <summary>The message to show.</summary>
        [JsonProperty("message", Required = Required.Always)]
        [Required]
        [StringLength(500, MinimumLength = 1)]
        public string Message { get; set; }
    
        /// <summary>The image to show (optional).</summary>
        [JsonProperty("list_image", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string ListImage { get; set; }
        
        /// <summary>Name of the cover image</summary>
        [JsonProperty("cover_image", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string CoverImage { get; set; }
        
        /// <summary>List of assigned notification images</summary>
        [JsonProperty("image", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public JRaw Image { get; set; }
    }
}