using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace z5.ms.common.assets.common
{
    /// <summary>Licensing details</summary>
    public class Licensing 
    {
        /// <summary>The name of the content owner.</summary>
        [JsonProperty("content_owner", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string ContentOwner { get; set; }
    
        /// <summary>The names of the countries where the item is available.</summary>
        [JsonProperty("license_countries", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public List<string> LicenseCountries { get; set; }
        
        /// <summary>The date and time from when the item is available.</summary>
        [JsonProperty("license_from", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? LicenseFrom { get; set; }
    
        /// <summary>The date and time until when the item is available.</summary>
        [JsonProperty("license_until", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? LicenseUntil { get; set; }
        
        /// <summary>The licensing periods in countries where the item is available.</summary>
        [JsonProperty("licensing_periods", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<string, LicensingPeriod> LicensingPeriods { get; set; }
    }

    /// <summary>Licensing details</summary>
    public class LicensingPeriod
    {
        /// <summary>The date and time from when the item is available.</summary>
        [JsonProperty("license_from", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? LicenseFrom { get; set; }

        /// <summary>The date and time until when the item is available.</summary>
        [JsonProperty("license_until", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? LicenseUntil { get; set; }

        /// <summary>Asset business type, e.g. Free, Ad</summary>
        /// <remarks>Items with business_type=Ad require client app to display ad to the customer before wathing the movie</remarks>
        [JsonProperty("business_type", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(StringEnumConverter))]
        public BusinessType? BusinessType { get; set; }

        /// <summary>A price tier at which the asset should be available in this country</summary>
        [JsonProperty("tier", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string TvodTier { get; set; }
    }
}