using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using z5.ms.common.abstractions;
using z5.ms.common.assets;

namespace z5.ms.domain.subscription.subscriptionplan

{
    /// <summary>Get list of Donation Subscription Plans for a given country</summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class GetDonationPlansQuery : IRequest<Result<List<SubscriptionPlan>>>
    {
        /// <summary>The country parameter filters items from lists that are not licensed in this given country</summary>
        [FromQuery(Name = "country")]
        [Required]
        public string Country { get; set; }

        /// <summary>The desired language the feed metadata should have as two letter “ISO 639-1” code</summary>
        [FromQuery(Name = "translation")]
        public string Translation { get; set; }
    }

    /// <summary>Subscription plan query parameters validator</summary>
    public class GetDonationPlansQueryValidator : AbstractValidator<GetDonationPlansQuery>
    {
        /// <inheritdoc />
        public GetDonationPlansQueryValidator()
        {
            RuleFor(m => m.Country).Must(BeValidCountryCode).WithMessage("Please specify a valid two-letter country code");
            RuleFor(m => m.Translation).Must(BeValidIso639CodeOrNull).WithMessage("Please specify a valid ISO 639-1 language code");
        }

        private static bool BeValidCountryCode(string language)
            => language != null && language.Length == 2;

        private static bool BeValidIso639CodeOrNull(string language)
            => language == null
               || language.Length == 2
               && CultureInfo
                   .GetCultures(CultureTypes.NeutralCultures)
                   .Any(ci => string.Equals(ci.TwoLetterISOLanguageName, language,
                       StringComparison.OrdinalIgnoreCase));
    }
}