using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace z5.ms.domain.subscription.viewmodel
{ 
    /// <summary>Add new device request model</summary>
    public class AddDevice 
    { 
        /// <summary>The human readable name of the device</summary>
        [JsonProperty("name")]
        [Required(ErrorMessage = "name is required")]
        public string Name { get; set; }

        /// <summary>The unique identifier of the device - created by the client.</summary>
        [JsonProperty("identifier")]
        [Required(ErrorMessage = "identifier is required")]
        public string Identifier { get; set; }
    }
}
