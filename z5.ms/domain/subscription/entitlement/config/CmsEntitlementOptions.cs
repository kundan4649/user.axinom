using System;

namespace z5.ms.domain.subscription.entitlement.config
{
    /// <summary>
    /// Configuration options for CMS based entitlements
    /// </summary>
    [Obsolete("Use entitlement2")]
    public class CmsEntitlementOptions
    {
        /// <summary>Secret token to validate entitlements for CMS</summary>
        public string CmsSecretToken { get; set; }
    }
}