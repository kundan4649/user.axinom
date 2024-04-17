using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace z5.ms.common.assets
{
    /// <summary>A list of EPG program data grouped by channel.</summary>
    [GeneratedCode("NJsonSchema", "9.6.5.0 (Newtonsoft.Json v9.0.0.0)")]
    public class EpgList 
    {
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
    
        /// <summary>EPG channels list</summary>
        [JsonProperty("items", Required = Required.Always)]
        [Required]
        public List<EpgChannel> Items { get; set; } = new List<EpgChannel>();
    }
}