using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace z5.ms.common.assets.common
{
    /// <summary>The genre identification</summary>
    public class Genre
    {
        /// <summary>Genre ID</summary>
        [JsonProperty("id", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string Id { get; set; }
    
        /// <summary>Translated genre text</summary>
        [JsonProperty("value", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string Value { get; set; }
        
        /// <summary>Internal method for testing of genres</summary>
        public static List<Genre> FromStrings(params string[] genres)
            => genres.Select(g => new Genre {Id = g, Value = g}).ToList();
        
        /// <inheritdoc />
        public override string ToString() => $"[Genre {Id}-{Value}]";
    }
}