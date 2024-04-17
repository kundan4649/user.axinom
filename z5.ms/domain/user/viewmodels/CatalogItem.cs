using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using z5.ms.common.assets.common;

namespace z5.ms.domain.user.viewmodels
{
    /// <summary>Catalog item view model</summary>
    public class CatalogItem
    {
        /// <summary>The unique ID of the catalog item.</summary>
        [JsonProperty("id", Required = Required.Always)]
        [Required]
        public string AssetId { get; set; }

        /// <summary>Unique asset type</summary>
        [JsonProperty("asset_type", Required = Required.Always, NullValueHandling = NullValueHandling.Ignore)]
        [DefaultValue(AssetType.Unknown)]
        public AssetType AssetType { get; set; }

        /// <summary>The playback position of this item in seconds if available. Defaults to zero.</summary>
        [JsonProperty("duration", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Populate)]
        [DefaultValue(0)]
        public int Duration { get; set; }

        /// <summary>Date when the item was added to the list. Defaults to server side UTC "now".</summary>
        [JsonProperty("date", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public DateTime Date { get; set; }

    }
}
