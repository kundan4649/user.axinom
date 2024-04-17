using System;
using FluentValidation;
using z5.ms.domain.subscription.entitlement.common;
using z5.ms.domain.subscription.entitlement.viewmodel;

namespace z5.ms.domain.subscription.entitlement.cms
{
    /// <summary>
    /// Entitlement query for CMS 
    /// </summary>
    [Obsolete("Use entitlement2")]
    public class GetCmsEntitlementQuery : GetEntitlementQuery
    {
        /// <inheritdoc />
        public GetCmsEntitlementQuery(EntitlementRequest entitlementRequest, string clientIp) : base(entitlementRequest, Guid.Empty, clientIp) {}
    }

    /// <inheritdoc />
    [Obsolete("Use entitlement2")]
    public class GetCmsEntitlementQueryValidator : GetEntitlementQueryValidator<GetCmsEntitlementQuery>
    {
        /// <inheritdoc />
        public GetCmsEntitlementQueryValidator()
        {
            RuleFor(q => q.Token).NotEmpty();
        }
    }
}