using System;
using System.ComponentModel.DataAnnotations;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using z5.ms.common;
using z5.ms.common.abstractions;
using z5.ms.domain.subscription.viewmodel;

namespace z5.ms.domain.subscription.crm
{ 
    /// <summary>
    /// 
    /// </summary>
    public class CrmUpdatePurchaseRequest : IRequest<Result<PurchaseInternal>>
    {
        /// <summary> 
        /// The ID of the purchase
        /// </summary>
        [FromRoute(Name = "id")]
        public Guid? Id { get; set; }

        /// <summary>
        /// Update request body
        /// </summary>
        [FromBody]
        [Required]
        public PurchaseUpdate PurchaseUpdate { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class PurchaseUpdate
    {
        /// <summary>
        /// If this is present it will move the purchase to a different user.
        /// </summary>
        /// <value>If this is present it will move the purchase to a different user.</value>
        [JsonProperty("customer_id", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public Guid? CustomerId { get; set; }

        /// <summary>
        /// If this is present it will change the purchase to a different purchase plan.
        /// </summary>
        /// <value>If this is present it will change the purchase to a different purchase plan.</value>
        [JsonProperty("subscription_plan_id", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string SubscriptionPlanId { get; set; }

        /// <summary>
        /// If this is present it will change the purchased asset to a different one.
        /// </summary>
        /// <value>If this is present it will change the purchased asset to a different one.</value>
        [JsonProperty("asset_id", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string AssetId { get; set; }

        /// <summary>
        /// If this is present it will change the purchase end date to a new one.
        /// </summary>
        /// <value>If this is present it will change the purchase end date to a new one.</value>
        [JsonProperty("purchase_end", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? PurchaseEnd { get; set; }
    }


        /// <summary>Queryvalidator</summary>
        public class CrmUpdatePurchaseRequestValidator : AbstractValidator<CrmUpdatePurchaseRequest>
    {
        /// <inheritdoc />
        public CrmUpdatePurchaseRequestValidator()
        {
            RuleFor(m => m.Id).NotEmpty().WithMessage("Invalid id");
            RuleFor(m => m.PurchaseUpdate.CustomerId).Must(BeAGuidOrNull).WithMessage("Invalid customer_id");
            RuleFor(m => m.PurchaseUpdate.SubscriptionPlanId).Must(BeAssetIdOrNull).WithMessage("Invalid subsription_plan_id");
            RuleFor(m => m.PurchaseUpdate.AssetId).Must(BeAssetIdOrNull).WithMessage("Invalid asset_id");
        }

        private static bool BeAGuidOrNull(Guid? guid) => guid == null || guid != default(Guid);

        private static bool BeAssetIdOrNull(string id) => string.IsNullOrWhiteSpace(id) || id.IsAssetId();
    }
}
