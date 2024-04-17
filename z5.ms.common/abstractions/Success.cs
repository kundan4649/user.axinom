using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace z5.ms.common.abstractions
{
    /// <summary>Success response model</summary>
    public class Success
    {
        /// <summary>The success code. Used in the frontends as resource key if a message should be shown to the user.</summary>
        [JsonProperty("code", Required = Required.Always)]
        [Required]
        public int? Code { get; set; }

        /// <summary>The message in English language. Not to be shown to the user - used for logging or better understanding. Optional.</summary>
        [JsonProperty("message", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string Message { get; set; }
    }
}