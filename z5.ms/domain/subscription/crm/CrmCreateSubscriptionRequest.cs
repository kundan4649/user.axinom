using System;
using System.ComponentModel.DataAnnotations;
using FluentValidation;
using MediatR;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using z5.ms.common.abstractions;
using z5.ms.domain.subscription.viewmodel;

namespace z5.ms.domain.subscription.crm
{
    /// <summary>CRM request to create a subscription</summary>
    public class CrmCreateSubscriptionRequest : IRequest<Result<SubscriptionInternal>>
    {
        /// <summary>The unique ID of the user who will own this subscription.</summary>
        [JsonProperty("customer_id")]
        [Required]
        public Guid CustomerId { get; set; }

        /// <summary>The unique ID of the subscription plan this subscription will be associated with.</summary>
        [JsonProperty("subscription_plan_id")]
        [Required]
        public string SubscriptionPlanId { get; set; }

        /// <summary>End date for the subscription.</summary>
        [JsonProperty("subscription_end")]
        [Required]
        public DateTime SubscriptionEnd { get; set; }

        /// <summary>IP address of the user</summary>
        [JsonProperty("ip_address", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string IpAddress { get; set; }

        /// <summary>Country in "ISO 3166-1 alpha-2" format from where the user initially registered.</summary>
        [JsonProperty("country", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string RegistrationCountry { get; set; }

        /// <summary>Set recurring status</summary>
        [JsonProperty("recurring_enabled")]
        public bool? RecurringEnabled { get; set; }

        /// <summary>Additional information for user to be stored in DB for reporting purposes</summary>
        [JsonProperty("additional", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public JObject Additional { get; set; }
    }


    /// <summary>Queryvalidator</summary>
    public class CrmCreateSubscriptionRequestValidator : AbstractValidator<CrmCreateSubscriptionRequest>
    {
        /// <inheritdoc />
        public CrmCreateSubscriptionRequestValidator()
        {
            RuleFor(m => m.SubscriptionEnd).NotEmpty().WithMessage("subscription_end is required");
            RuleFor(m => m.SubscriptionEnd).Must(BeAFutureDate).WithMessage("subscription_end must be a date in the future");
            RuleFor(m => m.CustomerId).Must(BeAGuid).WithMessage("customer_id is required");
            RuleFor(m => m.SubscriptionPlanId).NotEmpty().WithMessage("subscription_plan_id is required");
        }

        private static bool BeAFutureDate(DateTime date) => date.Date >= DateTime.UtcNow.Date;

        private static bool BeAGuid(Guid guid) => guid != default(Guid);
    }
}