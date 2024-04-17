using System;
using FluentValidation;
using Newtonsoft.Json;
using z5.ms.domain.subscription.entitlement.common;
using z5.ms.domain.subscription.entitlement.viewmodel;

namespace z5.ms.domain.subscription.entitlement.google
{
    /// <summary>
    /// Entitlement query for Google Playstore subscriptions 
    /// </summary>
    [Obsolete("Use entitlement2")]
    public class GetGoogleEntitlementQuery : GetEntitlementQuery
    {
        /// <inheritdoc />
        public GetGoogleEntitlementQuery(EntitlementRequest entitlementRequest, string clientIp) : base(entitlementRequest, Guid.Empty, clientIp)
        {
            PlaystoreSubscriptionId = entitlementRequest.ExtId;
        }

        /// <summary>Required if the entitlement should be checked based on a Google playstore subscription. The ID of the Google playstore subscription product.</summary>
        [JsonProperty("playstore_subscription_id", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string PlaystoreSubscriptionId { get; set; }       
    }

    /// <inheritdoc />
    [Obsolete("Use entitlement2")]
    public class GetGoogleEntitlementQueryValidator : GetEntitlementQueryValidator<GetGoogleEntitlementQuery>
    {
        /// <inheritdoc />
        public GetGoogleEntitlementQueryValidator()
        {
            RuleFor(q => q.Token).NotEmpty();
            RuleFor(q => q.PlaystoreSubscriptionId).NotEmpty();
        }
    }
}