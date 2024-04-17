using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using z5.ms.common.assets.common;

namespace z5.ms.common.assets
{
    /// <summary>A list of search results</summary>
    public class SearchList : IPagedList
    {
        /// <summary>The total number of EPG programs found by the query.</summary>
        [JsonProperty("epg_total", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public int? EpgTotal { get; set; }
        
        /// <summary>The total number of albums programs found by the query.</summary>
        [JsonProperty("albums_total", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public int? AlbumsTotal { get; set; }
    
        /// <summary>The total number of EPG programs found by the query.</summary>
        [JsonProperty("tvshows_total", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public int? TvshowsTotal { get; set; }
    
        /// <summary>The total number of EPG programs found by the query.</summary>
        [JsonProperty("movies_total", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public int? MoviesTotal { get; set; }

        /// <summary>The total number of live events found by the query.</summary>
        [JsonProperty("liveevents_total", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public int? LiveEventsTotal { get; set; }

        /// <summary>The page of the result set (one based).</summary>
        [JsonProperty("page", Required = Required.Always)]
        [Range(1.0, double.MaxValue)]
        public int Page { get; set; }
    
        /// <summary>Max amount of items returned per page.</summary>
        [JsonProperty("page_size", Required = Required.Always)]
        [Range(1.0, 100.0)]
        public int PageSize { get; set; }
    
        /// <summary>List of EPG programs matching the search query</summary>
        [JsonProperty("epg", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public List<ListItem> Epg { get; set; }
        
        /// <summary>List of albums programs matching the search query</summary>
        [JsonProperty("albums", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public List<ListItem> Albums { get; set; }
    
        /// <summary>List of TV shows matching the search query</summary>
        [JsonProperty("tvshows", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public List<ListItem> Tvshows { get; set; }
    
        /// <summary>List of movies matching the search query</summary>
        [JsonProperty("movies", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public List<ListItem> Movies { get; set; }

        /// <summary>List of live events matching the search query</summary>
        [JsonProperty("liveevents", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public List<ListItem> LiveEvents { get; set; }
    }
}