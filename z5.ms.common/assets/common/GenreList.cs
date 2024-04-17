using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace z5.ms.common.assets.common
{
    /// <summary>Genres list view model</summary>
    public class GenreList 
    {
        /// <summary>Genres list</summary>
        [JsonProperty("genres", Required = Required.Always)]
        [Required]
        public List<Genre> Genres { get; set; } = new List<Genre>();
    }
}