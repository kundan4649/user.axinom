using System;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using z5.ms.common.abstractions;
using z5.ms.domain.subscription.viewmodel;

namespace z5.ms.domain.subscription.queries
{
    /// <summary>Get list of Payments for a given criteria</summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class GetPaymentsQuery : PagingQuery<PaymentsInternal, PaymentInternalSortByField>
    {
        /// <summary>Filter the Payments for a specific customer.</summary>
        [FromQuery(Name = "customer_id")]
        public string CustomerId { get; set; }

        /// <summary>Filter by a specific Payment plan.</summary>
        [FromQuery(Name = "subscription_plan_id")]
        public string SubscriptionPlanId { get; set; }

        /// <summary>Filter by subscription ID.</summary>
        [FromQuery(Name = "subscription_id")]
        public string SubscriptionId { get; set; }

        /// <summary>Filter by purchase ID.</summary>
        [FromQuery(Name = "purchase_id")]
        public string PurchaseId { get; set; }

        ///// <summary>Filter by donation ID.</summary>
        //[FromQuery(Name = "donation_id")]
        //public string DonationId { get; set; }

        /// <summary>Filter by a specific payment provider</summary>
        [FromQuery(Name = "payment_provider")]
        public string PaymentProvider { get; set; }

        /// <summary>Filter by date, only return payments after this time.</summary>
        [FromQuery(Name = "date_from")]
        public DateTime? DateFrom { get; set; }

        /// <summary>Filter by date, only return payments before this time.</summary>
        [FromQuery(Name = "date_until")]
        public DateTime? DateUntil { get; set; }
    }

    /// <summary>Payments query parameters validator</summary>
    public class GetPaymentsQueryValidator : PagingQueryValidator<GetPaymentsQuery, PaymentInternalSortByField, PaymentsInternal>
    { 
        public GetPaymentsQueryValidator()
        {
            RuleFor(q => q.CustomerId).Must(BeValidGuid).WithMessage("Please specify a valid customer ID");
            RuleFor(q => q.SubscriptionId).Must(BeValidGuid).WithMessage("Please specify a valid subscription ID");
            RuleFor(q => q.PurchaseId).Must(BeValidGuid).WithMessage("Please specify a valid purchase ID");
            //RuleFor(q => q.DonationId).Must(BeValidGuid).WithMessage("Please specify a valid donation ID");
        }

        private static bool BeValidGuid(string guid)
            => guid == null || Guid.TryParse(guid, out _);
    }
}