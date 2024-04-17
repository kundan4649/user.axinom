using Newtonsoft.Json;

namespace z5.ms.common.assets.common
{
    /// <summary>
    /// Parent asset reference, used for child-parent relationships for episodes and seasons. 
    /// </summary>
    public class ParentReference
    {
        /// <summary>The unique ID of the parent (TV show, season etc.)</summary>
        [JsonProperty("id", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string Id { get; set; }
    }
}