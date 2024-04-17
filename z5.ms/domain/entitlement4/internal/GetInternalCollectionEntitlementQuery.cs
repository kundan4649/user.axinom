using z5.ms.domain.entitlement4.common;
using z5.ms.domain.entitlement4.viewmodel;

namespace z5.ms.domain.entitlement4.@internal
{
    /// <summary> Collection Entitlement query</summary>
    public class GetInternalCollectionEntitlementQuery : GetCollectionEntitlementQuery
    {
        /// <inheritdoc />
        public GetInternalCollectionEntitlementQuery(EntitlementRequest request, string clientIp) : base(request, clientIp) {}
    }

    /// <inheritdoc />
    public class
        GetInternalCollectionEntitlementQueryValidator : GetCollectionEntitlementQueryValidator<
            GetInternalCollectionEntitlementQuery>
    {
        /// <inheritdoc />
        public GetInternalCollectionEntitlementQueryValidator(){}
    }
}