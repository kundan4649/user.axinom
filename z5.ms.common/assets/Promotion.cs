using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace z5.ms.common.assets
{
    /// <summary>
    /// Response model for promotion details
    /// </summary>
    public class Promotion
    {
        /// <summary>The use case for the promo code</summary>
        [JsonProperty("title", Required = Required.Always)]
        [Required]
        [StringLength(50, MinimumLength = 1)]
        public string Title { get; set; }

        /// <summary>The random code the user has to enter to get the promotion</summary>
        [JsonProperty("code", Required = Required.Always)]
        [Required]
        [StringLength(12, MinimumLength = 6)]
        public string Code { get; set; }

        /// <summary>Start date time when the promo code can be used</summary>
        [JsonProperty("start_date", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? StartDate { get; set; }

        /// <summary>End date when the promo code can be used. Date without time component</summary>
        [JsonProperty("end_date", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? EndDate { get; set; }

        /// <summary>The discount in percent that should be applied to the payments.Expected to be zero and the plan to already have a reduced price.</summary>
        [JsonProperty("discount", Required = Required.Always)]
        [Range(0, 100.0)]
        public double Discount { get; set; }

        /// <summary>Type of the discount (can be percentage or currency)</summary>
        [JsonProperty("discount_type", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(StringEnumConverter))]
        public DiscountType DiscountType { get; set; }

        /// <summary> Maximum number of billing cycles which can be used to get a promo discount. </summary>
        [JsonProperty("number_billing_cycles", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public int BillingCycles { get; set; } = 1;
       
        /// <summary>Terms and conditions regarding the current promotion</summary>
        [JsonProperty("terms_and_conditions", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string TermsAndConditions { get; set; }

        // TODO: check this desc.
        /// <summary>The discount in percent that should be applied to the payments.Expected to be zero and the plan to already have a reduced price.</summary>
        [JsonIgnore]
        public bool IsActive => StartDate < DateTime.UtcNow && EndDate > DateTime.UtcNow;

        /// <summary>Determines if free trial can be used when a promotion is also present</summary>
        [JsonProperty("freetrial_with_promotion_allowed", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public bool FreeTrialWithPromotionAllowed { get; set; } = true;

        /// <summary>Determines if this promotion can be used again provided there are enough allowed billing cycles left</summary>
        [JsonProperty("multiple_usage_allowed", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public bool MultipleUsageAllowed { get; set; }

        /// <summary>The audience this promotion is intended for</summary>
        [JsonProperty("target_users", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(StringEnumConverter))]
        public TargetAudience TargetUsers { get; set; } = TargetAudience.All;
        
        public double CalculateDiscount(double price)
            => Math.Round(DiscountType == DiscountType.Percentage ? price * Discount / 100 : Discount, 2);
    }
}
