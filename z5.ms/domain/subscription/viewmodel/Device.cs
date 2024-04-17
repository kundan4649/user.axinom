using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace z5.ms.domain.subscription.viewmodel
{ 
    /// <summary>Response model for device</summary>
    public class Device
    { 
        /// <summary>The unique database ID of the device</summary>
        [JsonProperty("id", Required = Required.Always)]
        [Required]
        public Guid Id { get; set; }

        /// <summary>The human readable name of the device</summary>
        [JsonProperty("name", Required = Required.Always)]
        [Required]
        public string Name { get; set; }

        /// <summary>The unique identifier of the device - created by the client.</summary>
        [JsonProperty("identifier", Required = Required.Always)]
        [Required]
        public string Identifier { get; set; }

        /// <summary>The date and time when the device was added</summary>
        [JsonProperty("create_date", Required = Required.Always)]
        [Required]
        public DateTime CreateDate { get; set; }
    }
}
