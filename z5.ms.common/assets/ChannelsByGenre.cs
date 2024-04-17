using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using z5.ms.common.assets.common;

namespace z5.ms.common.assets
{
    /// <summary>
    /// Generic type to list items
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ItemList<T>
    {
        /// <summary>A generic list of items</summary>
        [JsonProperty("items", Required = Required.Always)]
        [Required]
        public List<T> Items { get; set; }
    }

    /// <summary> List of channel genres </summary>
    public class GenreChannelList
    {
        /// <summary>Genre ID</summary>
        [JsonProperty("id", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string Id { get; set; }

        /// <summary>Translated genre text</summary>
        [JsonProperty("title", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string Title { get; set; }

        /// <summary>Genres list</summary>
        [JsonProperty("items", Required = Required.Always)]
        [Required]
        public List<ListItem> Items { get; set; }
    }
}
