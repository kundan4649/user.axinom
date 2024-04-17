using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using z5.ms.common.assets.common;

namespace z5.ms.common.assets
{
    /// <summary>
    /// External Link response object, which is returned by MS public APIs whenever a detailed info is needed on a External Link
    /// </summary>
    public class ExternalLink
    {
        /// <summary>The unique ID</summary>
        [JsonProperty("id", Required = Required.Always)]
        [Required]
        public string Id { get; set; }

        /// <summary>Type of the asset (live event)</summary>
        [JsonProperty("asset_type", Required = Required.Always)]
        public AssetType AssetType { get; set; }

        /// <summary>Human readable title</summary>
        [JsonProperty("title", Required = Required.Always)]
        [Required]
        [StringLength(50, MinimumLength = 1)]
        public string Title { get; set; }

        /// <summary>Detailed description of the live event</summary>
        [JsonProperty("description", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string Description { get; set; }

        /// <summary>Short description of the live event, to be used in list views</summary>
        [JsonProperty("short_description", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string ShortDescription { get; set; }

        /// <summary>The URL for the external resource</summary>
        [JsonProperty("url", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string Url { get; set; }

        /// <summary>The target where the URL should be opened.</summary>
        [JsonProperty("target", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(StringEnumConverter))]
        public ExternalLinkTarget Target { get; set; }

        /// <summary>Image for external link</summary>
        [JsonProperty("image", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public JRaw Image { get; set; }

        /// <summary>Country of origin</summary>
        [JsonProperty("countries", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public IList<string> Countries { get; set; }
        
        /// <summary>Available languages</summary>
        [JsonProperty("languages", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public IList<string> Languages { get; set; }
    }
}