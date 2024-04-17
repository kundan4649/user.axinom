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
    /// <summary>
    /// 
    /// </summary>
    public class CrmCreatePurchaseRequest : IRequest<Result<PurchaseInternal>>
    {
        /// <summary>
        /// The unique ID of the user who will own this purchase.
        /// </summary>
        /// <value>The unique ID of the user who will own this purchase.</value>
        [JsonProperty("customer_id")]
        [Required]
        public Guid? CustomerId { get; set; }

        /// <summary>
        /// The unique ID of the purchase plan this purchase will be associated with.
        /// </summary>
        /// <value>The unique ID of the purchase plan this purchase will be associated with.</value>
        [JsonProperty("subscription_plan_id")]
        [Required]
        public string SubscriptionPlanId { get; set; }

        /// <summary>
        /// The ID of the asset for the purchase
        /// </summary>
        /// <value>The ID of the asset for the purchase</value>
        [JsonProperty("asset_id")]
        [Required]
        public string AssetId { get; set; }

        /// <summary>
        /// The date at which point the purchase ends.
        /// </summary>
        /// <value>The date at which point the purchase ends.</value>
        [JsonProperty("purchase_end", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? PurchaseEnd { get; set; }

        /// <summary>IP address of the user</summary>
        [JsonProperty("ip_address", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string IpAddress { get; set; }

        /// <summary>Country in "ISO 3166-1 alpha-2" format from where the user initially registered.</summary>
        [JsonProperty("country", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string RegistrationCountry { get; set; }

        /// <summary>Additional information for user to be stored in DB for reporting purposes</summary>
        [JsonProperty("additional", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public JObject Additional { get; set; }
    }

    /// <summary>Queryvalidator</summary>
    public class CrmCreatePurchaseRequestValidator : AbstractValidator<CrmCreatePurchaseRequest>
    {
        /// <inheritdoc />
        public CrmCreatePurchaseRequestValidator()
        {
            RuleFor(m => m.CustomerId).NotEmpty().WithMessage("Invalid customer_id");
            RuleFor(m => m.SubscriptionPlanId).NotEmpty().WithMessage("subscription_plan_id is required");
            RuleFor(m => m.AssetId).NotEmpty().WithMessage("asset_id is required");
            RuleFor(m => m.PurchaseEnd).NotEmpty().Must(BeAFutureDate).WithMessage("purchase_end must be a date in the future");
        }

        private static bool BeAFutureDate(DateTime? date) => (date != null && date.Value.Date >= DateTime.UtcNow.Date);
    }
}
