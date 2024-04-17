using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace z5.ms.domain.subscription.viewmodel
{
    /// <summary>Tax that should be applied for payments</summary>
    public class Tax
    {
        /// <summary>The name or abbreviation of the tax</summary>
        [JsonProperty("name", Required = Required.Always)]
        [Required]
        public string Name { get; set; }

        /// <summary>The percentage of the tax</summary>
        [JsonProperty("percentage", Required = Required.Always)]
        public double Percentage { get; set; }

    }
}
