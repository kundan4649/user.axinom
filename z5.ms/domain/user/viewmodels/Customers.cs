using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace z5.ms.domain.user.viewmodels
{
    /// <summary>Paged list response type of customers</summary>
    public class Customers
    {
        /// <summary>The total number of customers found by the query.</summary>
        [JsonProperty("total", Required = Required.Always)]
        public int Total { get; set; }

        /// <summary>The page of the result set (one based).</summary>
        [JsonProperty("page", Required = Required.Always)]
        [Range(1.0, double.MaxValue)]
        public int Page { get; set; }

        /// <summary>How many customers should be returned per page.</summary>
        [JsonProperty("page_size", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        [Range(1.0, 100.0)]
        public int? PageSize { get; set; }

        /// <summary>List of customers.</summary>
        [JsonProperty("customers", Required = Required.Always)]
        [Required]
        public List<Customer> CustomerList { get; set; } = new List<Customer>();
    }
}
