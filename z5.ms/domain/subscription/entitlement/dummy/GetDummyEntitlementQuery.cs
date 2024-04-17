using System;
using z5.ms.domain.subscription.entitlement.common;
using z5.ms.domain.subscription.entitlement.viewmodel;

namespace z5.ms.domain.subscription.entitlement.dummy
{
    /// <summary>
    /// Dummy Entitlement query 
    /// </summary>
    [Obsolete("Use entitlement2")]
    public class GetDummyEntitlementQuery : GetEntitlementQuery
    {
        /// <inheritdoc />
        public GetDummyEntitlementQuery(EntitlementRequest entitlementRequest, string clientIp) : base(entitlementRequest, Guid.Empty, clientIp)
        {
        }
    }

    /// <inheritdoc />
    [Obsolete("Use entitlement2")]
    public class GetDummyEntitlementQueryValidator : GetEntitlementQueryValidator<GetDummyEntitlementQuery>
    {
    }
}