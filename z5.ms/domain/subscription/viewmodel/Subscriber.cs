using System;
using Newtonsoft.Json;

namespace z5.ms.domain.subscription.viewmodel
{
    /// <summary>A customer response for the internal api</summary>
    public class Subscriber
    {
        /// <summary>The unique ID of the customer.</summary>
        [JsonProperty("id", Required = Required.Always)]
        public Guid Id { get; set; }

        /// <summary>The customer's first name</summary>
        [JsonProperty("first_name", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string FirstName { get; set; }

        /// <summary>The customer's last name.</summary>
        [JsonProperty("last_name", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string LastName { get; set; }
    }
}
