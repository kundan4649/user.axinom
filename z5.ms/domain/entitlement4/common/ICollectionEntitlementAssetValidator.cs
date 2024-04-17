using System.Collections.Generic;
using System.Threading.Tasks;
using z5.ms.common.abstractions;

namespace z5.ms.domain.entitlement4.common
{
    /// <summary> Service to validate collection assets entitlements</summary>
    public interface ICollectionEntitlementAssetValidator
    {
        /// <summary>Validate the drm key of an asset and licensing</summary>
        Task<Result<List<Result<EntitlementAssetValidationResult>>>> ValidateDrmAsset(string assetId, string country = null);
    }
}