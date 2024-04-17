using System;
using System.ComponentModel.DataAnnotations;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using z5.ms.common.abstractions;
using z5.ms.domain.subscription.viewmodel;

namespace z5.ms.domain.subscription.crm
{
    /// <summary>Crm subscription update request</summary>
    public class CrmUpdateSubscriptionRequest : IRequest<Result<SubscriptionInternal>>
    {
        /// <summary>Unique id of the subscription to update</summary>
        [FromRoute(Name = "id")]
        [Required]
        public Guid SubscriptionId { get; set; }

        /// <summary>Parameters describing changes to be made to a subscription</summary>
        [FromBody]
        [Required]
        public SubscriptionUpdate SubscriptionUpdate { get; set; }
    }

    /// <summary>Crm subscription update request</summary>
    public class CrmUpdateSubscriptionRequestByUser : IRequest<Result<SubscriptionInternal>>
    {
        /// <summary>The ID of the user from whom to find the subscription</summary>
        [FromQuery(Name = "customer_id")]
        [Required]
        public Guid CustomerId { get; set; }

        /// <summary>The ID of the subscription plan to find</summary>
        [FromQuery(Name = "subscription_plan_id")]
        [Required]
        public string SubscriptionPlanId { get; set; }

        /// <summary>Parameters describing changes to be made to a subscription</summary>
        [FromBody]
        [Required]
        public SubscriptionUpdate SubscriptionUpdate { get; set; }
    }

    /// <summary>Parameters describing changes to be made to a subscription</summary>
    public class SubscriptionUpdate
    {
        /// <summary>End date for the subscription</summary>
        [JsonProperty("subscription_end")]
        public DateTime? SubscriptionEnd { get; set; }

        /// <summary>If this is present it will move the subscription to a different user.</summary>
        [JsonProperty("customer_id")]
        public Guid? CustomerId { get; set; }

        /// <summary>If this is present it will change the subscription to a different subscription plan.</summary>
        [JsonProperty("subscription_plan_id")]
        public string SubscriptionPlanId { get; set; }

        /// <summary>Set recurring status</summary>
        [JsonProperty("recurring_enabled")]
        public bool? RecurringEnabled { get; set; }

        /// <summary>On how to get the value depends on the payment provider:</summary>
        /// <remarks>For PayTM it is the "payment_id" value. </remarks>
        /// <remarks>For Adyen it is teh the "pspReference". </remarks>
        /// <remarks>For Billdesk it is the third field from their string which is "TxnReferenceNo".</remarks>
        /// <remarks>For Fortumo it is the "subscription_id".</remarks>
        /// <remarks>For Google the "purchaseToken".</remarks>
        /// <remarks>For Apple the "latest_receipt" property valu.</remarks>
        [JsonProperty("identifier")]
        public string Identifier { get; set; }

        /// <summary>State of the subscription</summary>
        [JsonProperty("state")]
        public SubscriptionState? State { get; set; }

        /// <summary>Additional information for user to be stored in DB for reporting purposes</summary>
        [JsonProperty("additional")]
        public JObject Additional { get; set; }
    }

    /// <summary>Queryvalidator</summary>
    public class CrmUpdateSubscriptionRequestValidator : AbstractValidator<CrmUpdateSubscriptionRequest>
    {
        /// <inheritdoc />
        public CrmUpdateSubscriptionRequestValidator()
        {
            RuleFor(m => m.SubscriptionId).NotEmpty().WithMessage("Subscription id is required");

            RuleFor(m => m.SubscriptionUpdate.SubscriptionEnd).Must(BeNullOrAFutureDate).WithMessage("subscription_end may not be a date in the past");
        }

        private static bool BeNullOrAFutureDate(DateTime? date) => date == null || date.Value.Date >= DateTime.UtcNow.Date;
    }

    /// <summary>Queryvalidator</summary>
    public class CrmUpdateSubscriptionRequestByUserValidator : AbstractValidator<CrmUpdateSubscriptionRequestByUser>
    {
        /// <inheritdoc />
        public CrmUpdateSubscriptionRequestByUserValidator()
        {
            RuleFor(m => m.CustomerId).NotEmpty().WithMessage("Customer id is required");

            RuleFor(m => m.SubscriptionPlanId).NotEmpty().WithMessage("Subscription plan id is required");

            RuleFor(m => m.SubscriptionUpdate.SubscriptionEnd).Must(BeNullOrAFutureDate).WithMessage("subscription_end may not be a date in the past");
        }

        private static bool BeNullOrAFutureDate(DateTime? date) => date == null || date.Value.Date >= DateTime.UtcNow.Date;
    }
}