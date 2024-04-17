using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace z5.ms.domain.subscription.viewmodel
{ 
    /// <summary>Update device request model</summary>
    public class UpdateDevice
    { 
        /// <summary>The unique database ID of the device</summary>
        [JsonProperty("id", Required = Required.Always)]
        [Required(ErrorMessage = "id is required")]
        public Guid Id { get; set; }

        /// <summary>The human readable name of the device</summary>
        [JsonProperty("name", Required = Required.Always)]
        [Required(ErrorMessage = "name is required")]
        public string Name { get; set; }
    }
}
