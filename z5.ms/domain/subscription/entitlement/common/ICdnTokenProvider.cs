using System;

namespace z5.ms.domain.subscription.entitlement.common
{
    // TODO: this is currently based on Edgecast CDN token generation requirements for DC, it will probably need some generalization at some point
    /// <summary>
    /// Interface for CDN token providers 
    /// </summary>
    [Obsolete("Use entitlement2")]
    public interface ICdnTokenProvider
    {
        /// <summary>Name/type of the CDN token provider in lowercase e.g.edgecast</summary>
        string ProviderName { get;}

        /// <summary>Generate a CDN token</summary>
        /// <param name="clientIp">Client IP for which the token will be valid</param>
        /// <param name="allowedUrls">The URL (pattern) for which the token is valid</param>
        /// <returns></returns>
        string GetToken(string clientIp, string allowedUrls);
    }
}