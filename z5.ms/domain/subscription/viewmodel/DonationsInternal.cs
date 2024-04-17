using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace z5.ms.domain.subscription.viewmodel
{
    /// <summary>A donation response for the internal api</summary>
    public class DonationsInternal
    {
        /// <summary>The total number of donations found by the query.</summary>
        [JsonProperty("total", Required = Required.Always)]
        public int Total { get; set; }

        /// <summary>The page of the result set (one based).</summary>
        [JsonProperty("page", Required = Required.Always)]
        [Range(1.0, double.MaxValue)]
        public int Page { get; set; }

        /// <summary>How many donations should be returned per page.</summary>
        [JsonProperty("page_size", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        [Range(1.0, 100.0)]
        public int? PageSize { get; set; }

        /// <summary>List of donations</summary>
        [JsonProperty("donations", Required = Required.Always)]
        [Required]
        public List<DonationInternal> DonationList { get; set; } = new List<DonationInternal>();
    }
}
