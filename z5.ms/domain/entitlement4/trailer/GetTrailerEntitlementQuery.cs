using System;
using FluentValidation;
using z5.ms.domain.entitlement4.common;
using z5.ms.domain.entitlement4.viewmodel;

namespace z5.ms.domain.entitlement4.trailer
{
    // TODO: remove unnecessary properties (DRM-related)
    /// <summary>
    /// Entitlement query for trailers
    /// </summary>
    public class GetTrailerEntitlementQuery : GetEntitlementQuery
    {
        /// <inheritdoc />
        public GetTrailerEntitlementQuery(EntitlementRequest entitlementRequest, string clientIp) : base(entitlementRequest, Guid.Empty, clientIp) {}
    }

    /// <inheritdoc />
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