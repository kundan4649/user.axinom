using System.Collections.Generic;
using Newtonsoft.Json;

namespace z5.ms.common.assets.common
{
    /// <summary>Paged item list with reduced data (for litings)</summary>
    /// <remarks>closed constucted type</remarks>
    public class PagedItemList : PagedItemList<ListItem> { }
    
    /// <inheritdoc />
    /// <summary>Paged item list</summary>
    public class PagedItemList<T> : IPagedList
    {
        /// <summary>Total number of results</summary>
        [JsonProperty("total", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public int Total { get; set; }

        /// <inheritdoc />
        [JsonProperty("page", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public int Page { get; set; }

        /// <inheritdoc />
        [JsonProperty("page_size", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public int PageSize { get; set; }

        /// <summary>Items returned by the query</summary>
        [JsonProperty("items", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public IEnumerable<T> Items { get; set; }
    }
}