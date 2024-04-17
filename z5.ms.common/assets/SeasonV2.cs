using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using z5.ms.common.assets.common;

namespace z5.ms.common.assets
{
    /// <summary>
    /// The V2 season which is part of a TV show
    /// </summary>
    public class SeasonV2 : Season
    {
        /// <summary>Related TV shows in the desired sort order.</summary>
        [JsonProperty("related_tvshows", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public List<ListItem> RelatedTvShows { get; set; }

        /// <summary>Related movies in the desired sort order.</summary>
        [JsonProperty("related_movies", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public List<ListItem> RelatedMovies { get; set; }

        /// <summary>Related collections in the desired sort order.</summary>
        [JsonProperty("related_collections", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public List<ListItem> RelatedCollections { get; set; }

        /// <summary>Related channels in the desired sort order.</summary>
        [JsonProperty("related_channels", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public List<ListItem> RelatedChannels { get; set; }

        /// <summary>Gets or Sets Extended</summary>
        [JsonProperty("extended", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public JRaw Extended { get; set; }
    }
}
