using System;

namespace z5.ms.domain.subscription.entitlement.common
{
    /// <summary>
    /// Entitlement validation result for asset validator
    /// </summary>
    [Obsolete("Use entitlement2")]
    public class EntitlementAssetValidationResult
    {
        /// <summary>Boolean result to indicate asset can be display without subscription validation (for free, ads type assets)</summary>
        public bool Entitled { get; set; }

        /// <summary>Id of requested asset</summary>
        public string AssetId { get; set; }

        /// <summary>Type of requested asset</summary>
        public int AssetType { get; set; }
    }
}
