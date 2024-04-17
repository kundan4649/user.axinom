using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace z5.ms.domain.subscription.viewmodel
{
    /// <summary>A purchase response for the internal api</summary>
    public class PurchasesInternal
    {
        /// <summary>The total number of purchases found by the query.</summary>
        [JsonProperty("total", Required = Required.Always)]
        public int Total { get; set; }

        /// <summary>The page of the result set (one based).</summary>
        [JsonProperty("page", Required = Required.Always)]
        [Range(1.0, double.MaxValue)]
        public int Page { get; set; }

        /// <summary>How many purchases should be returned per page.</summary>
        [JsonProperty("page_size", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        [Range(1.0, 100.0)]
        public int? PageSize { get; set; }

        /// <summary>List of purchases</summary>
        [JsonProperty("purchases", Required = Required.Always)]
        [Required]
        public List<PurchaseInternal> PurchaseList { get; set; } = new List<PurchaseInternal>();
    }
}
