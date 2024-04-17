using System;
using FluentValidation;
using z5.ms.domain.entitlement4.common;
using z5.ms.domain.entitlement4.viewmodel;

namespace z5.ms.domain.entitlement4.cms
{
    /// <summary>
    /// Entitlement query for CMS 
    /// </summary>
    public class GetCmsEntitlementQuery : GetEntitlementQuery
    {
        /// <inheritdoc />
        public GetCmsEntitlementQuery(EntitlementRequest entitlementRequest, string clientIp) : base(entitlementRequest, Guid.Empty, clientIp) {}
    }

    /// <inheritdoc />
    public class GetCmsEntitlementQueryValidator : GetEntitlementQueryValidator<GetCmsEntitlementQuery>
    {
        /// <inheritdoc />
        public GetCmsEntitlementQueryValidator()
        {
            RuleFor(q => q.Token).NotEmpty();
        }
    }
}