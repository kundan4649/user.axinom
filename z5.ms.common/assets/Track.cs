using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace z5.ms.common.assets
{
    /// <summary>
    /// Audio album track data structure
    /// </summary>
    public class Track
    {
        /// <summary>The unique ID</summary>
        [JsonProperty("id", Required = Required.Always)]
        [Required]
        public string Id { get; set; }

        /// <summary>Human readable title</summary>
        [JsonProperty("title", Required = Required.Always)]
        [Required]
        [StringLength(50, MinimumLength = 1)]
        public string Title { get; set; }

        /// <summary>Actors or voice actors</summary>
        [JsonProperty("artists", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public List<string> Artists { get; set; }

        /// <summary>track index</summary>
        [JsonProperty("index", Required = Required.Always)]
        public int Index { get; set; }

        /// <summary>Duration in seconds</summary>
        [JsonProperty("duration", Required = Required.Always)]
        public int Duration { get; set; }

        /// <summary>audio file url</summary>
        [JsonProperty("url", Required = Required.Always)]
        [Required]
        public string Url { get; set; }
    }
}