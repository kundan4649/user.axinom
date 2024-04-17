using System;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using z5.ms.common.abstractions;
using z5.ms.common.attributes;
using z5.ms.domain.subscription.viewmodel;

namespace z5.ms.domain.subscription.queries
{
    /// <summary>Get list of Purchases for a given criteria</summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class GetPurchasesQuery : PagingQuery<PurchasesInternal, PurchaseInternalSortByField>
    {
        /// <summary>Filter by (partial) ID.</summary>
        [FromQuery(Name = "id")]
        public string Id { get; set; }

        /// <summary>Filter the Purchases for a specific customer.</summary>
        [FromQuery(Name = "customer_id")]
        public string CustomerId { get; set; }

        /// <summary>Filter by a specific Purchase plan.</summary>
        [FromQuery(Name = "subscription_plan_id")]
        public string SubscriptionPlanId { get; set; }

        /// <summary>Filter by a specific payment provider</summary>
        [FromQuery(Name = "payment_provider")]
        public string PaymentProvider { get; set; }

        /// <summary>Filter by a recurring status</summary>
        [FromQuery(Name = "asset_id")]
        public string AssetId { get; set; }

        /// <summary>Filter by free trial support</summary>
        [FromQuery(Name = "asset_type")]
        public int? AssetType { get; set; }

        /// <summary>Filter by state; whether the Purchase has expired or not</summary>
        [FromQueryEnumMember(Name = "state")]
        public SearchPurchaseState? State { get; set; }
    }

    /// <summary>Purchases query parameters validator</summary>
    public class GetPurchasesQueryValidator : PagingQueryValidator<GetPurchasesQuery, PurchaseInternalSortByField, PurchasesInternal>
    {
        public GetPurchasesQueryValidator()
        {
            RuleFor(q => q.Id).Must(BeValidGuid).WithMessage("Please specify a valid ID");
            RuleFor(q => q.CustomerId).Must(BeValidGuid).WithMessage("Please specify a valid customer ID");
        }

        private static bool BeValidGuid(string guid)
            => guid == null || Guid.TryParse(guid, out _);
    }
}