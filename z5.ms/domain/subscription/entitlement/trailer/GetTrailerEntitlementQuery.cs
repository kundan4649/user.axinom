using System;
using FluentValidation;
using z5.ms.domain.subscription.entitlement.common;
using z5.ms.domain.subscription.entitlement.viewmodel;

namespace z5.ms.domain.subscription.entitlement.trailer
{
    // TODO: remove unnecessary properties (DRM-related)
    /// <summary>
    /// Entitlement query for trailers
    /// </summary>
    [Obsolete("Use entitlement2")]
    public class GetTrailerEntitlementQuery : GetEntitlementQuery
    {
        /// <inheritdoc />
        public GetTrailerEntitlementQuery(EntitlementRequest entitlementRequest, string clientIp) : base(entitlementRequest, Guid.Empty, clientIp) {}
    }

    /// <inheritdoc />
    [Obsolete("Use entitlement2")]
    public class GetTrailerEntitlementQueryValidator : AbstractValidator<GetTrailerEntitlementQuery>
    {
        /// <inheritdoc />
        public GetTrailerEntitlementQueryValidator()
        {
            RuleFor(q => q.RequestType).Equal(EntitlementRequestType.Cdn)
                .WithMessage("Only CDN request is supported for entitlement provider 'trailer'");
        }
    }
}