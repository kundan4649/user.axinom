using System;
using z5.ms.domain.subscription.entitlement.common;
using z5.ms.domain.subscription.entitlement.viewmodel;

namespace z5.ms.domain.subscription.entitlement.@internal
{
    /// <summary>
    /// Entitlement query for internal subscriptions and purchases
    /// </summary>
    [Obsolete("Use entitlement2")]
    public class GetInternalEntitlementQuery : GetEntitlementQuery
    {
        /// <inheritdoc />
        public GetInternalEntitlementQuery(EntitlementRequest entitlementRequest, Guid userId, string clientIp) : base(entitlementRequest, userId, clientIp) {}
    }

    /// <inheritdoc />
    [Obsolete("Use entitlement2")]
    public class GetInternalEntitlementQueryValidator : GetEntitlementQueryValidator<GetInternalEntitlementQuery> {}
}