using System;
using FluentValidation;
using z5.ms.common;
using z5.ms.common.assets.common;
using z5.ms.domain.entitlement4.common;
using z5.ms.domain.entitlement4.viewmodel;

namespace z5.ms.domain.entitlement4.@internal
{
    /// <summary>
    /// Entitlement query for internal subscriptions and purchases
    /// </summary>
    public class GetInternalEntitlementQuery : GetEntitlementQuery
    {
        /// <summary>
        /// API version
        /// </summary>
        public int Version;

        /// <inheritdoc />
        public GetInternalEntitlementQuery(EntitlementRequest entitlementRequest, Guid userId, string clientIp, int version = 0) : base(entitlementRequest, userId, clientIp)
        {
            Version = version;
        }
    }

    /// <inheritdoc />
    public class GetInternalEntitlementQueryValidator : GetEntitlementQueryValidator<GetInternalEntitlementQuery>
    {
        /// <inheritdoc />
        public GetInternalEntitlementQueryValidator()
        {
            RuleFor(q => q.AssetId.GetAssetTypeOrDefault(99))
                .Must(type =>
                    (short) type == (short) AssetType.Movie || (short) type == (short) AssetType.Episode ||
                    (short) type == (short) AssetType.Channel)
                .WithName("asset_id")
                .WithMessage("Use supported assets only (Movie, Episode or Channel)");
        }
    }
}