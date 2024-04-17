using System;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using z5.ms.common;
using z5.ms.common.abstractions;
using z5.ms.common.attributes;
using z5.ms.domain.subscription.viewmodel;

namespace z5.ms.domain.subscription.queries
{
    /// <summary>Get list of Subscriptions for a given criteria</summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class GetSubscriptionsQuery : PagingQuery<SubscriptionsInternal, SubscriptionInternalSortByField>
    {
        /// <summary>Filter by (partial) ID.</summary>
        [FromQuery(Name = "id")]
        public string Id { get; set; }

        /// <summary>Filter the subscriptions for a specific customer.</summary>
        [FromQuery(Name = "customer_id")]
        public string CustomerId { get; set; }

        /// <summary>Filter by a specific subscription plan.</summary>
        [FromQuery(Name = "subscription_plan_id")]
        public string SubscriptionPlanId { get; set; }

        /// <summary>Filter by a specific payment provider</summary>
        [FromQuery(Name = "payment_provider")]
        public string PaymentProvider { get; set; }

        /// <summary>Filter by state; whether the subscription has expired or not</summary>
        [FromQueryEnumMember(Name = "state")]
        public SearchSubscriptionState? State { get; set; }

        /// <summary>Filter by a recurring status</summary>
        [FromQuery(Name = "recurring_enabled")]
        public bool? RecurringEnabled { get; set; }

        /// <summary>Filter by free trial support</summary>
        [FromQuery(Name = "free_trial")]
        public bool? FreeTrial { get; set; }
    }

    /// <summary>Subscriptions query parameters validator</summary>
    public class GetSubscriptionsQueryValidator : PagingQueryValidator<GetSubscriptionsQuery, SubscriptionInternalSortByField, SubscriptionsInternal>
    {
        public GetSubscriptionsQueryValidator()
        {
            RuleFor(q => q.Id).Must(BeValidGuid).WithMessage("Please specify a valid subscription ID");
            RuleFor(q => q.CustomerId).Must(BeValidGuid).WithMessage("Please specify a valid customer ID");
            RuleFor(q => q.SubscriptionPlanId).Must(BeAnAssetId).WithMessage("Please specify a valid asset ID");
        }

        private static bool BeAnAssetId(string id)
            => string.IsNullOrWhiteSpace(id) || id.IsAssetId();

        private static bool BeValidGuid(string guid)
            => guid == null || Guid.TryParse(guid, out _);
        
    }
}