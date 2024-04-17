﻿using System;
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
using z5.ms.common.infrastructure.assetcache;

namespace z5.ms.domain.subscription.subscriptionplan
{
    /// <summary>Get subscription plans with the valid promotions</summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class GetPromoSubscriptionPlanQueryV2 : IRequest<Result<List<SubscriptionPlan>>>
    {
        /// <summary>Promotion code</summary>
        [FromRoute(Name = "code")]
        [Required]
        public string Code { get; set; }

        /// <summary>The country parameter filters items from lists that are not licensed in this given country</summary>
        [FromQuery(Name = "country")]
        [Required]
        public string Country { get; set; }

        /// <summary>The desired language the feed metadata should have as two letter “ISO 639-1” code</summary>
        [JsonProperty("translation", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        [FromQuery(Name = "translation")]
        public string Translation { get; set; }
        
        /// <summary>The name of the field by which to sort</summary>
        [JsonProperty("sort_by_field", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        [FromQuery(Name = "sort_by_field")] 
        public string SortByField { get; set; } = "priority"; //title, duration, price, priority
        
        /// <summary>The sort order of the "sort by field"</summary>
        [JsonProperty("sort_order", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        [FromQuery(Name = "sort_order")]
        public string SortOrder { get; set; } = "asc"; //asc, desc
    }

    /// <summary>SubscriptionPlan query parameters validator</summary>
    public class GetPromoSubscriptionPlanQueryV2Validator : AbstractValidator<GetPromoSubscriptionPlanQueryV2>
    {
        private readonly string[] _sortByFields = {"title", "duration", "price", "priority"};

        /// <inheritdoc />
        public GetPromoSubscriptionPlanQueryV2Validator()
        {
            RuleFor(m => m.Code).NotEmpty();
            RuleFor(m => m.Country).Must(BeValidCountryCode).WithMessage("Please specify a valid two-letter country code");
            RuleFor(m => m.Translation).Must(BeValidIso639CodeOrNull).WithMessage("Please specify a valid ISO 639-1 language code");  
            RuleFor(m => m.SortByField).Must(s => s.IsInCollectionOrNull(_sortByFields)).WithMessage("Unsupported sort field name");
            RuleFor(m => m.SortOrder).Must(s => s.IsInCollectionOrNull("asc", "desc")).WithMessage("Unsupported sort field name");
        }

        private static bool BeValidCountryCode(string country)
            => country != null && country.Length == 2;

        private static bool BeValidIso639CodeOrNull(string language)
            => language == null
               || language.Length == 2
               && CultureInfo
                   .GetCultures(CultureTypes.NeutralCultures)
                   .Any(ci => string.Equals(ci.TwoLetterISOLanguageName, language,
                       StringComparison.OrdinalIgnoreCase));
    }
}