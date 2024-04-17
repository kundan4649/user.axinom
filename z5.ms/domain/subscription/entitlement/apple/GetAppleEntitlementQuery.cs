using System;
using FluentValidation;
using z5.ms.domain.subscription.entitlement.common;
using z5.ms.domain.subscription.entitlement.viewmodel;

namespace z5.ms.domain.subscription.entitlement.apple
{
    /// <summary>
    /// Entitlement query for Apple App Store subscriptions 
    /// </summary>
    [Obsolete("Use entitlement2")]
    public class GetAppleEntitlementQuery : GetEntitlementQuery
    {
        /// <inheritdoc />
        public GetAppleEntitlementQuery(EntitlementRequest entitlementRequest, string clientIp) : base(entitlementRequest, Guid.Empty, clientIp)
        {
        }
    }

    /// <inheritdoc />
    [Obsolete("Use entitlement2")]
    public class GetAppleEntitlementQueryValidator : GetEntitlementQueryValidator<GetAppleEntitlementQuery>
    {
        /// <inheritdoc />
        public GetAppleEntitlementQueryValidator()
        {
            RuleFor(q => q.Token).NotEmpty();
        }
    }
}