using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace z5.ms.domain.subscription.viewmodel
{
    /// <summary>Custom subscription prepare request model for Billdesk</summary>
    public class BilldeskPrepareSubscription : RegisterSubscription
    {
        /// <summary>Billdesk payment type</summary>
        [JsonProperty("paymenttype", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string PaymentType { get; set; }

        /// <summary>Billdesk payment code</summary>
        [JsonProperty("paymentcode", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public int? PaymentCode { get; set; }
    }

    /// <summary>Custom purchase prepare request model for Billdesk</summary>
    public class BilldeskPreparePurchase : RegisterPurchase
    {
        /// <summary>Billdesk payment type</summary>
        [JsonProperty("paymenttype", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string PaymentType { get; set; }

        /// <summary>Billdesk payment code</summary>
        [JsonProperty("paymentcode", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public int? PaymentCode { get; set; }
    }
}
