using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using z5.ms.common.attributes;
using z5.ms.domain.user.viewmodels;

namespace z5.ms.domain.user.user
{
    /// <summary>Common request model for user registration</summary>
    public class RegisterUserCommand
    {
        /// <summary>Type of the authentication method</summary>
        [JsonIgnore]
        public virtual AuthenticationMethod Type { get; set; }

        /// <summary>Indicates if refresh token is requested</summary>
        [JsonIgnore]
        public bool Refresh { get; set; } = true;

        /// <summary>Version of the endpoint</summary>
        [JsonIgnore]
        public int Version { get; set; }

        /// <summary>Some mac address - e.g. the one from the device the user registered with.</summary>
        [JsonProperty("mac_address", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string MacAddress { get; set; }

        /// <summary>The IP address of the user. If it is missing the callers IP address will be used.</summary>
        [JsonProperty("ip_address", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        [IpAddress]
        public string IpAddress { get; set; }

        /// <summary>The country of the user. If it is missing the callers IP address will be used to find it in the maxmind DB.</summary>
        [JsonProperty("registration_country", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        [StringLength(2)]
        public string RegistrationCountry { get; set; }

        /// <summary>The specific region in the registration country. If it is missing the callers IP address will be used to find it in the maxmind DB.</summary>
        [JsonProperty("registration_region", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string RegistrationRegion { get; set; }

        /// <summary>Additional parameters meant for reporting purposes.</summary>
        [JsonProperty("additional", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public JObject Additional { get; set; }

        [JsonIgnore]
        public string Message = "Registration successful";
    }
}