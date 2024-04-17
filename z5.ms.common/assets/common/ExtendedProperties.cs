using Newtonsoft.Json;

namespace z5.ms.common.assets.common
{
    /// <summary>
    /// Data container used to selectively include and map values from the published 'extended' property of an asset. 
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class ExtendedProperties
    {
        /// <summary>Alternative title used to generate search engine optimised page titles</summary>
        [JsonProperty("seo_title", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string SeoTitle { get; set; }
        
        /// <summary>List of comma-separated digital keywords</summary>
        [JsonProperty("digital_keywords", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string DigitalKeywords { get; set; }
        
        /// <summary>Collection Auto Play</summary>
        [JsonProperty("collection_auto_play", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public bool? CollectionAutoPlay { get; set; }
        
        /// <summary>Meta title string</summary>
        [JsonProperty("meta_title", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string MetaTitle { get; set; }
        
        /// <summary>Meta description string</summary>
        [JsonProperty("meta_description", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string MetaDescription { get; set; }
        
        /// <summary>List of comma-separated meta keywords</summary>
        [JsonProperty("meta_keywords", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string MetaKeywords { get; set; }
        
        /// <summary>URL name for SEO optimization</summary>
        [JsonProperty("url_name", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string UrlName { get; set; }
    }
}