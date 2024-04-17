using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using z5.ms.common.assets.common;

namespace z5.ms.domain.user.viewmodels
{
    /// <summary>Reminder item view model</summary>
    public class ReminderItem
    {
        /// <summary>The unique ID of the catalog item.</summary>
        [JsonProperty("id", Required = Required.Always)]
        [Required]
        public string AssetId { get; set; }

        /// <summary>Unique asset type</summary>
        [JsonProperty("asset_type", Required = Required.Always, DefaultValueHandling = DefaultValueHandling.Populate)]
        [DefaultValue(AssetType.Unknown)]
        public AssetType AssetType { get; set; }

        /// <summary>Remind for upcoming EPG program via</summary>
        [JsonProperty("reminder_type", Required = Required.Always)]
        [Required]
        [JsonConverter(typeof(StringEnumConverter))]
        public ReminderType ReminderType { get; set; }

    }
}
