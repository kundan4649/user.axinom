using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using z5.ms.common;
using z5.ms.common.assets;
using z5.ms.common.assets.common;
using z5.ms.common.extensions;

namespace z5.ms.domain.subscription.subscriptionplan
{
    /// <inheritdoc cref="EntityBase" />
    /// <summary>Subscription Plan aggregate</summary>
    public class SubscriptionPlanEntity : EntityBase, IAggregateRoot
    {
        /// <summary>
        /// Type of the subscription plan. The plan may for making purchases or donations rather than subscriptions.
        /// </summary>
        [JsonProperty("subscription_plan_type", Required = Required.Always, NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(StringEnumConverter))]
        public SubscriptionPlanType SubscriptionPlanType { get; set; }

        /// <summary>The system from where the user registered from (internal or B2B customers)</summary>
        [JsonProperty("system", Required = Required.Always)]
        [Required]
        public string System { get; set; }

        /// <summary>Type/duration of an individual billing cycle, e.g. day, week, month, quarter, year; subset of values is defined in CMS per project. Default 1 day</summary>
        [JsonProperty("billing_cycle_type", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(StringEnumConverter))]
        public BillingCycleType? BillingCycleType { get; set; }

        /// <summary>The duration in days how long a subscription can be used before it must be paid again.</summary>
        [JsonProperty("billing_frequency", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        [Range(1.0, double.MaxValue)]
        public int? BillingFrequency { get; set; }

        /// <summary>The amount of money that a subscription costs for the duration of the billing frequency. This includes taxes.</summary>
        [JsonProperty("price", Required = Required.Always)]
        [Range(0.0, 9999999.0)]
        public double Price { get; set; }

        /// <summary>The currency that should be used.</summary>
        [JsonProperty("currency", Required = Required.Always)]
        [Required]
        [StringLength(int.MaxValue, MinimumLength = 1)]
        public string Currency { get; set; }

        /// <summary>In which (single) country is this subscription plan available.</summary>
        [JsonProperty("country", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        [StringLength(2, MinimumLength = 2)]
        public string Country { get; set; }

        /// <summary>In which countries is this subscription plan available.</summary>
        [JsonProperty("countries", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        [StringLength(2, MinimumLength = 2)]
        public List<string> Countries { get; set; }

        /// <summary>Start date when this subscription plan can be purchased.</summary>
        [JsonProperty("start", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? Start { get; set; }

        /// <summary>End date when this subscription plan is not purchasable anymore.</summary>
        [JsonProperty("end", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? End { get; set; }

        /// <summary>If the subscription end date is reached - should the next payment be conducted or not.</summary>
        [JsonProperty("only_available_with_promotion", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public bool OnlyAvailableWithPromotion { get; set; }

        /// <summary>Is this subscription plan a one-time purchase or a recurring version.</summary>
        [JsonProperty("recurring", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public bool Recurring { get; set; } = true;

        /// <summary>Available payment providers for this plan</summary>
        [JsonProperty("payment_providers", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public List<PlanPaymentProviderEntity> PaymentProviderEntities { get; set; }

        /// <summary>Available promotions for this plan</summary>
        [JsonProperty("promotions", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public List<Promotion> Promotions { get; set; }

        /// <summary>Defines the asset types that can be watched as part of this subscription plan.</summary>
        [JsonProperty("asset_types", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public List<int> AssetTypes { get; set; }

        /// <summary>Defines the asset IDs that can be watched/bought as part of this subscription plan.
        /// This should not be exposed to the frontends in the catalog as it can be very large.</summary>
        [JsonProperty("asset_ids", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public List<string> AssetIds { get; set; }

        /// <summary>If different than 0, then it defines the number of days before the first payment will be charged.</summary>
        [JsonProperty("free_trial", Required = Required.Default)]
        public int FreeTrial { get; set; }

        /// <summary>User must be logged in to use this subscription plan for entitlement (only applies to FVOD / AVOD)</summary>
        [JsonProperty("only_available_for_logged_in_users", Required = Required.Default)]
        public bool? OnlyAvailableForLoggedInUsers { get; set; }

        /// <summary>Content associated with this subscription plan can be downloaded for later viewing</summary>
        [JsonProperty("is_downloadable", Required = Required.Default)]
        public bool? IsDownloadable { get; set; }

        /// <summary>Number of hours after live broadcast that channel playback is supported. (Not checked by entitlement)</summary>
        [JsonProperty("catch_up_hours", Required = Required.Default)]
        public int? CatchUpHours { get; set; }

        /// <summary>Number of devices that user can use at the same time</summary>
        [JsonProperty("number_of_supported_devices", Required = Required.Default)]
        public int NumOfSupportedDevices { get; set; }

        /// <summary>A definition for TVOD purchase plans what kind of pricing tier assets can be purchased with that plan.</summary>
        [JsonProperty("tier", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string TvodTier { get; set; }

        /// <summary>Priority value, 1 by default</summary>
        [JsonProperty("priority", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public int Priority { get; set; } = 1;

        /// <summary>Defines the audio languages of the movie item to be played as part of the subscription plan.</summary>
        [JsonProperty("movie_audio_languages", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public List<string> MovieAudioLanguages { get; set; }

        /// <summary>Defines the audio languages of the episode item to be played as part of the subscription plan.</summary>
        [JsonProperty("tv_show_audio_languages", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public List<string> TvShowAudioLanguages { get; set; }

        /// <summary>Defines the audio languages of the channel item to be played as part of the subscription plan.</summary>
        [JsonProperty("channel_audio_languages", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public List<string> ChannelAudioLanguages { get; set; }

        /// <summary>Custom text value for subscription duration. Used when we want to add promotional text for subscription plan</summary>
        [JsonProperty("duration_text", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string DurationText { get; set; }

        /// <summary>Promotional text for subscribing to subscription plan. Included in response email</summary>
        [JsonProperty("promo_text_email", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string PromoTextEmail { get; set; }

        /// <summary>Promotional text for subscribing to subscription plan. Included in response sms</summary>
        [JsonProperty("promo_text_sms", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string PromoTextSMS { get; set; }

        /// <summary>Indicates the subscription plan is valid for all configured countries in the plan</summary>
        [JsonProperty("valid_for_all_countries", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public bool ValidForAllCountries { get; set; } = true; 
        
        /// <summary>Terms and conditions regarding the current plan</summary>
        [JsonProperty("terms_and_conditions", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string TermsAndConditions { get; set; }
        
        /// <summary>Check whether this subscription plan is active at a given UTC datetime</summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public bool IsAvailableAt(DateTime date) => (Start == null || date >= Start) && (End == null || date < End);

        /// <summary>Is this purchase plan linked to a given asset (either by AssetIds or by AssetTypes).</summary>
        /// <remarks>If assetType is omitted it will be determined from the assetId.</remarks>
        /// <param name="assetId"></param>
        /// <param name="assetType"></param>
        /// <returns></returns>
        public bool IncludesAsset(string assetId, int? assetType = null) =>
            AssetIds.IsNotNullAndContains(assetId) || AssetTypes.IsNotNullAndContains(assetType ?? assetId.GetAssetTypeOrDefault());

        /// <summary>Returns the available country from Country or Countries properties of subscription plan.</summary>
        public string AvailableCountry => !string.IsNullOrWhiteSpace(Country) ? Country : (Countries?.FirstOrDefault() ?? "ZZ");

        /// <summary>Is subscription plan associated with a queried country?</summary>
        public bool ContainsCountry(string country)
            => !string.IsNullOrWhiteSpace(Country) ? !string.IsNullOrWhiteSpace(country) && country.Equals(Country, StringComparison.OrdinalIgnoreCase)
                : Countries?.Any(c => c.Equals(country, StringComparison.OrdinalIgnoreCase))
                  ?? country?.Equals("ZZ", StringComparison.OrdinalIgnoreCase) ?? false;

        /// <summary>Calculate end date of the subscription according to start date</summary>
        public DateTime? CalculateEndDateTime(DateTime startDateTime)
        {
            if (BillingFrequency == null || BillingFrequency < 1)
                return null;

            switch (BillingCycleType)
            {
                case z5.ms.common.assets.BillingCycleType.Hours:
                    return startDateTime.AddHours((int)BillingFrequency);
                case z5.ms.common.assets.BillingCycleType.Months:
                    return startDateTime.AddMonths((int)BillingFrequency);
                default:
                    return startDateTime.AddDays((int)BillingFrequency);
            }
        }

        /// <summary>Get a promotion using the code from defined promotions of the subscription plan</summary>
        public Promotion GetPromotion(string promoCode)
            => Promotions.FirstOrDefault(x => x.Code.EqualsIgnoreCase(promoCode) && x.IsActive);
    }

    /// <summary>
    /// Payment provider subscription data that is used when making payment requests.
    /// </summary>
    public class PlanPaymentProviderEntity
    {
        /// <summary>The name of the payment provider</summary>
        [JsonProperty("name", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        /// <summary>The reference to the product in the payment provider system (if such reference is needed).</summary>
        [JsonProperty("product_reference", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string ProductReference { get; set; }

        /// <summary>The secret code of the product in the payment provider system</summary>
        [JsonProperty("secret", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string ProductSecret { get; set; }
    }
}