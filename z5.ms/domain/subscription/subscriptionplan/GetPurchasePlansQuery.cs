using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using z5.ms.common;
using z5.ms.common.abstractions;
using z5.ms.common.assets;

namespace z5.ms.domain.subscription.subscriptionplan

{
    /// <summary>Get list of Subscription Plans linked to an asset</summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class GetPurchasePlansQuery : IRequest<Result<List<SubscriptionPlan>>>
    {
        /// <summary>The asset to get prices for</summary>
        [FromQuery(Name ="asset_id")]
        [Required]
        public string AssetId { get; set; }

        /// <summary>The country parameter filters items from lists that are not licensed in this given country</summary>
        [FromQuery(Name = "country")]
        [Required]
        public string Country { get; set; }

        /// <summary>Filter results available with a specified promoCode</summary>
        [FromQuery(Name = "code")]
        public string PromoCode { get; set; }

        /// <summary>The system for which to return subscription plans </summary>
        [FromQuery(Name = "system")]
        public string System { get; set; } = "Internal";

        /// <summary>The desired language the feed metadata should have as two letter “ISO 639-1” code</summary>
        [FromQuery(Name = "translation")]
        public string Translation { get; set; }
    }

    /// <summary>Subscription plan query parameters validator</summary>
    public class GetPurchasePlansQueryValidator : AbstractValidator<GetPurchasePlansQuery>
    {
        /// <inheritdoc />
        public GetPurchasePlansQueryValidator()
        {
            RuleFor(m => m.AssetId).Must(BeAnAssetId).WithMessage("Please specify a valid asset ID");
            RuleFor(m => m.Country).Must(BeValidCountryCode).WithMessage("Please specify a valid two-letter country code");
            RuleFor(m => m.Translation).Must(BeValidIso639CodeOrNull).WithMessage("Please specify a valid ISO 639-1 language code");
        }

        private static bool BeValidCountryCode(string language)
            => language != null && language.Length == 2;

        private static bool BeAnAssetId(string id) => id.IsAssetId();

        private static bool BeValidIso639CodeOrNull(string language)
            => language == null
               || language.Length == 2
               && CultureInfo
                   .GetCultures(CultureTypes.NeutralCultures)
                   .Any(ci => string.Equals(ci.TwoLetterISOLanguageName, language,
                       StringComparison.OrdinalIgnoreCase));
    }
}