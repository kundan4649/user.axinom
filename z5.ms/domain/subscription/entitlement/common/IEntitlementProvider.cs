using System;
using System.Collections.Generic;
using z5.ms.domain.subscription.entitlement.viewmodel;

namespace z5.ms.domain.subscription.entitlement.common
{
    /// <summary>
    /// Generic interface for entitlement providers
    /// </summary>
    [Obsolete("Use entitlement2")]
    public interface IEntitlementProvider
    {
        /// <summary>Check whether a user is entitled to access an asset</summary>
        /// <param name="assetId">ID of the asset to check</param>
        /// <param name="token">The token to use for entitlement check, it may be a user token (internal subscription/purchase), Google Playstore token or an Apple App Store token</param>
        /// <param name="extId">An external ID that may be required to to verify external entilement providers (e.g. the ID of a Google playstore subscription product)</param>
        /// <returns>True if the user entitled to access a resources, false otherwise</returns>
        bool Check(string assetId, string token, string extId = null);

        /// <summary>Get a DRM token for a DRM-protected asset</summary>
        /// <param name="assetId">ID of the asset to check</param>
        /// <param name="token">The token to use for entitlement check, it may be a user token (internal subscription/purchase), Google Playstore token or an Apple App Store token</param>
        /// <param name="extId">An external ID that may be required to to verify external entilement providers (e.g. the ID of a Google playstore subscription product)</param>
        /// <param name="persistent">Should the DRM license be persistent</param>
        /// <returns>DRM token</returns>
        string GetDrm(string assetId, string token, string extId = null, bool persistent = false);

        /// <summary>Get a CDN token for an asset</summary>
        /// <param name="assetId">ID of the asset to check</param>
        /// <param name="token">The token to use for entitlement check, it may be a user token (internal subscription/purchase), Google Playstore token or an Apple App Store token</param>
        /// <param name="extId">An external ID that may be required to to verify external entilement providers (e.g. the ID of a Google playstore subscription product)</param>
        /// <returns>CDN token</returns>
        IEnumerable<CdnToken> GetCdn(string assetId, string token, string extId = null);
    }
}