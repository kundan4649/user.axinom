using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace z5.ms.common.infrastructure.geoip
{
    /// <summary>
    /// Country data returned by the GeoIP controller
    /// </summary>
    public class Country
    {
        /// <summary>Country code in "ISO 3166-1 alpha-2" format</summary>
        [JsonProperty("country_code", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        [StringLength(2, MinimumLength = 2)]
        public string CountryCode { get; set; }

        /// <summary>The name of the country.</summary>
        [JsonProperty("country", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        [Required]
        public string CountryName { get; set; }

        /// <summary>The name of the state (if one is matched).</summary>
        [JsonProperty("state", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string State { get; set; }

        /// <summary>The time_zome of the state (if one is matched).</summary>
        [JsonProperty("time_zone_offset", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public double? TimeZoneOffset { get; set; }

        public static Country Unknown => new Country {CountryCode = "ZZ", State = ""};
    }
}
