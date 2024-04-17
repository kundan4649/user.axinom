using System;

namespace z5.ms.domain.entitlement4.common
{
    // TODO: possibly a premature generalization, sorry :(
    /// <summary>
    /// Interface for DRM token providers 
    /// </summary>
    public interface IDrmTokenProvider
    {
        /// <summary>Generate a DRM token</summary>
        /// <param name="isPersistent">Indicates whether a license should be persistent i.e. stored in the client for offline use</param>
        /// <param name="contentKeyId"></param>
        /// <param name="version">Entitlement API version</param>
        /// <param name="licenseExpiration">End date of the subscription the token is provided for</param> // TODO: looks very unfitting in this interface
        /// <returns>Generated DRM token</returns>
        string GetToken(bool isPersistent, string contentKeyId = null, int version = 0, DateTime? licenseExpiration = null);
    }
}