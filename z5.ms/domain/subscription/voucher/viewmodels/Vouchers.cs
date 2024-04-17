using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace z5.ms.domain.subscription.voucher.viewmodels
{ 
    /// <summary>View model for paged list of Vouchers</summary>
    public class Vouchers
    {
        /// <summary>The total number of vouchers found by the query.</summary>
        [JsonProperty("total", Required = Required.Always)]
        public int Total { get; set; }

        /// <summary>The page of the result set (one based).</summary>
        [JsonProperty("page", Required = Required.Always)]
        [Range(1.0, double.MaxValue)]
        public int? Page { get; set; }

        /// <summary>How many vouchers should be returned per page.</summary>
        [JsonProperty("page_size", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        [Range(1.0, 100.0)]
        public int? PageSize { get; set; }

        /// <summary>List of vouchers.</summary>
        [JsonProperty("vouchers", Required = Required.Always)]
        [Required]
        public List<Voucher> VoucherList { get; set; } = new List<Voucher>();
    }
}
