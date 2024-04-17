using System.Collections.Generic;
using System.Threading.Tasks;
using z5.ms.common.abstractions;
using z5.ms.domain.entitlement4.viewmodel;

namespace z5.ms.domain.entitlement4.@internal
{
    /// <summary>Interface for getting bulk drm data from collection</summary>
    public interface IInternalCollectionEntitlementProvider
    {
        /// <summary>Get bulk drm data from collection. </summary>
        /// <param name="query">Entitlement query parameters</param>
        /// <returns>DRM token</returns>
        Task<Result<List<CollectionEntitlementItem>>> GetBulkDrm(GetInternalCollectionEntitlementQuery query);
    }
}