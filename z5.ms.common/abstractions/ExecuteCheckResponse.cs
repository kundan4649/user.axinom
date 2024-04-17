using Newtonsoft.Json;

namespace z5.ms.common.abstractions
{
    /// <summary>Response type for Axinom SMS execute check </summary>
    public class ExecuteCheckResponse
    {
        /// <summary>Name of the check that is requested to execute</summary>
        [JsonProperty("Check")]
        public string Check { get; set; }

        /// <summary>Either the string "Success" or "Failure"</summary>
        [JsonProperty("Result")]
        public string Result { get; set; }

        /// <summary>A string with any additional information that the check wants to report to a human operator</summary>
        [JsonProperty("Message")]
        public string Message { get; set; }
    }
}
