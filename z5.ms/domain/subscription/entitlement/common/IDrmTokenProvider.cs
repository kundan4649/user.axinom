using System;

namespace z5.ms.domain.subscription.entitlement.common
{
    /// <summary>
    /// Interface for DRM token providers 
    /// </summary>
    [Obsolete("Use entitlement2")]
    public interface IDrmTokenProvider
    {
        /// <summary>Generate a DRM token</summary>
        /// <param name="persistent">Indicates whether a license should be persistent i.e. stored in the client for offline use</param>
        /// <returns>Generated DRM token</returns>
        string GetToken(bool persistent);
    }
}