using System;
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
    /// <summary>Get the details of a movie</summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class GetSubscriptionPlanQuery : IRequest<Result<SubscriptionPlan>>
    {
        /// <summary>The ID of the movie</summary>
        [FromRoute(Name = "id")]
        [Required]
        public string Id { get; set; }

        /// <summary>The desired language the feed metadata should have as two letter “ISO 639-1” code</summary>
        [FromQuery(Name = "translation")]
        public string Translation { get; set; }
    }

    /// <summary>SubscriptionPlan query parameters validator</summary>
    public class GetSubscriptionPlanQueryValidator : AbstractValidator<GetSubscriptionPlanQuery>
    {
        /// <inheritdoc />
        public GetSubscriptionPlanQueryValidator()
        {
            RuleFor(m => m.Id).NotEmpty();
            RuleFor(m => m.Id).Must(BeAnAssetId).WithMessage("Please specify a valid asset ID");
            RuleFor(m => m.Translation).Must(BeValidIso639CodeOrNull).WithMessage("Please specify a valid ISO 639-1 language code");
        }

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