using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using z5.ms.common.attributes;

namespace z5.ms.domain.subscription.viewmodel
{
    /// <summary>Request to register a subscription</summary>
    public class RegisterSubscription
    {
        /// <summary>The ID of the subscription plan to subscribe to</summary>
        [JsonProperty("subscription_plan_id")]
        [Required(ErrorMessage = "subscription_plan_id is required")]
        public string SubscriptionPlanId { get; set; }

        /// <summary>Optionally a promo code that should be applied to the purchase of the subscription plan.</summary>
        [JsonProperty("promo_code")]
        public string PromoCode { get; set; }

        /// <summary>Optionally a voucher code that should be applied to the purchase of the subscription plan.</summary>
        [JsonIgnore]
        public string VoucherCode { get; set; }

        /// <summary>The UI language as two letter "ISO 639-1" code. Default is English (en).</summary>
        [JsonProperty("language")]
        [StringLength(2, MinimumLength = 2, ErrorMessage = "language must be 2 characters")]
        public string Language { get; set; }

        /// <summary>The environment type (e.g. web or app) if needed for a payment provider.</summary>
        [JsonProperty("environment")]
        [JsonConverter(typeof(StringEnumConverter))]
        [Range(0, 2, ErrorMessage = "Invalid environment")]
        public PaymentEnvironment Environment { get; set; }

        /// <summary>The IP address of the user. If it is missing the callers IP address will be used.</summary>
        [JsonProperty("ip_address", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        [IpAddress]
        public string IpAddress { get; set; }

        /// <summary>The country of the user. If it is missing the callers IP address will be used to find it in the maxmind DB.</summary>
        [JsonProperty("country", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string RegistrationCountry { get; set; }

        /// <summary>The specific region in the country. If it is missing the callers IP address will be used to find it in the maxmind DB.</summary>
        [JsonProperty("region", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string RegistrationRegion { get; set; }

        /// <summary>Additional parameters meant for reporting purposes.</summary>
        [JsonProperty("additional", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public JObject Additional { get; set; }
    }
}
