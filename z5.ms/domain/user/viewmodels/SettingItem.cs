using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace z5.ms.domain.user.viewmodels
{
    /// <summary>Setting item view model</summary>
    public class SettingItem
    {
        /// <summary>The key of the setting</summary>
        [JsonProperty("key", Required = Required.Always)]
        [Required]
        public string SettingKey { get; set; }

        /// <summary>The value of the setting.</summary>
        [JsonConverter(typeof(JsonStringConverter))]
        [JsonProperty("value", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string SettingValue { get; set; }

    }
}
