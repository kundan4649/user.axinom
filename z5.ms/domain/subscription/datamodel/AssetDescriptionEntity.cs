using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace z5.ms.domain.subscription.datamodel
{
    /// <summary>Display data for an asset</summary>
    public class AssetDescriptionEntity
    {
        /// <summary>Unique asset ID</summary>
        [JsonProperty("id", Required = Required.Always, NullValueHandling = NullValueHandling.Ignore)]
        public string Id { get; set; }

        /// <summary>Human readable title for this item</summary>
        [JsonProperty("title", Required = Required.Always)]
        public string Title { get; set; }

        /// <summary>List of assigned asset images</summary>
        [JsonProperty("image", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public JRaw Image { get; set; }

        /// <summary>Asset description</summary>
        [JsonProperty("description", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string Description { get; set; }

        /// <summary>Asset short description</summary>
        [JsonProperty("short_description", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string ShortDescription { get; set; }

        /// <summary>Tvod pricing information</summary>
        [JsonProperty("tvod", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public TvodPricing TvodPricing { get; set; }
    }

    /// <summary>Tvod pricing information</summary>
    public class TvodPricing
    {
        /// <summary>A price tier at which the asset should be available in that country</summary>
        [JsonProperty("tier", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string TvodTier { get; set; }
    }
}