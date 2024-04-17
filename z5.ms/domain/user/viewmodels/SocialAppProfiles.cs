using Newtonsoft.Json;
using System.Collections.Generic;

namespace z5.ms.domain.user.viewmodels
{
    public class SocialAppProfiles
    {
        [JsonProperty("id", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string AppId { get; set; }
        [JsonProperty("name", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string AppName { get; set; }
        [JsonProperty("socialid", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string SocialProfileId { get; set; }
    }
}