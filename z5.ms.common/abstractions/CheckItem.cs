using Newtonsoft.Json;

namespace z5.ms.common.abstractions
{
    /// <summary>Check items of subscription service to be used with Axinom SMS</summary>
    public class CheckItem
    {
        /// <summary>Unique ID of the check, used to reference the check in other API operations</summary>
        [JsonProperty("Name")]
        public string Name { get; set; }

        /// <summary>Optional human-readable name title for the check. May be null or missing</summary>
        [JsonProperty("Title")]
        public string Title { get; set; }
    }
}
