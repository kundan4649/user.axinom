using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace z5.ms.domain.subscription.viewmodel
{
    /// <summary>View model for Refunds</summary>
    public class Refund
    {
        /// <summary>The unique database ID of the refund</summary>
        [JsonProperty("id", Required = Required.Always)]
        [Required]
        public Guid Id { get; set; }

        /// <summary>The unique id of the related payment</summary>
        [JsonProperty("payment_id", Required = Required.Always)]
        [Required]
        public Guid PaymentId { get; set; }

        /// <summary>The amount to be refunded</summary>
        [JsonProperty("amount", Required = Required.Always)]
        [Required]
        public double Amount { get; set; }

        /// <summary>The unique identifier of the refund from payment provider.</summary>
        [JsonProperty("external_identifier", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string ExternalIdentifier { get; set; }

        /// <summary>User name of the person who makes te refund request (eg. CMS username)</summary>
        [JsonProperty("requester", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string Requester { get; set; }

        /// <summary>Optional comment to provide information regarding the purpose of the refund</summary>
        [JsonProperty("comment", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string Comment { get; set; }

        /// <summary>The date and time when the refund is made</summary>
        [JsonProperty("date", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public DateTime Date { get; set; }
    }

    /// <summary>Refunds response for the internal api</summary>
    public class Refunds
    {
        /// <summary>The total number of refunds found by the query.</summary>
        [JsonProperty("total", Required = Required.Always)]
        public int Total { get; set; }

        /// <summary>The page of the result set (one based).</summary>
        [JsonProperty("page", Required = Required.Always)]
        [Range(1.0, double.MaxValue)]
        public int Page { get; set; }

        /// <summary>How many refunds should be returned per page.</summary>
        [JsonProperty("page_size", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        [Range(1.0, 100.0)]
        public int? PageSize { get; set; }

        /// <summary>List of refunds</summary>
        [JsonProperty("refunds", Required = Required.Always)]
        [Required]
        public List<Refund> RefundList { get; set; } = new List<Refund>();
    }
}

