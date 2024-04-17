using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace z5.ms.domain.user.datamodels
{
    public partial class HipiEndpointSettings
    {
        //[JsonProperty("EndPointDetails")]
        public List<EndPointDetail> EndPointDetails { get; set; }

        // [JsonProperty("Headers")]
        public Headers Headers { get; set; }
    }

    public partial class EndPointDetail
    {
        [JsonProperty("HttpMethod")]
        public string HttpMethod { get; set; }

        [JsonProperty("Endpoint")]
        public string Endpoint { get; set; }
    }

    public partial class Headers
    {
        // [JsonProperty("x-api-key")]
        public string XApiKey { get; set; }
    }

    public class HipiPutUserProfile
    {
        [JsonProperty("firstName")]
        public string FirstName { get; set; }
        [JsonProperty("lastName")]
        public string LastName { get; set; }
        //[JsonProperty("tag")]
        //public string Tag { get; set; } = "INFLUENCER";
        [JsonProperty("phoneNumber")]
        public string PhoneNumber { get; set; }
        [JsonProperty("dob")]
        public DateTime? Dob { get; set; }
        [JsonProperty("email")]
        public string Email { get; set; }
    }
}