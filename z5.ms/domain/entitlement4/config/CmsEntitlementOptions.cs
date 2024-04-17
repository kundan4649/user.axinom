namespace z5.ms.domain.entitlement4.config
{
    /// <summary>
    /// Configuration options for CMS based entitlements
    /// </summary>
    public class CmsEntitlementOptions
    {
        /// <summary>Secret token to validate entitlements for CMS</summary>
        public string CmsSecretToken { get; set; }
    }
}