using z5.ms.common;

namespace z5.ms.domain.entitlement4.config
{
    /// <summary>Catalog BC registration options</summary>
    public class EntitlementRegistrationOptions : IComponentRegistrationOptions
    {
        /// <summary>Use the internal subscription entitlement logic</summary>
        public bool UseSubscriptionEntitlement { get; set; }

        /// <summary>Use the internal purchase entitlement logic</summary>
        public bool UsePurchaseEntitlement { get; set; }

        /// <summary>Use the CMS entitlement logic (CMS is basically a super user, has access to everything)</summary>
        public bool UseCmsEntitlement { get; set; }

        /// <summary>Use the trailer entitlement logic (only CDN tokens)</summary>
        public bool UseTrailerEntitlement { get; set; }
        
        /// <summary>Use BuyDRM DRM token generation</summary>
        public bool UseBuyDrm { get; set; }
        
        /// <summary>Use AxinomDRM DRM token generation</summary>
        public bool UseAxinomDrm{ get; set; }
        
        /// <summary>Use Verizon Edgecast CDN token generation</summary>
        public bool UseEdgecastCdn { get; set; }

        /// <summary>Use Device validation logic</summary>
        public bool UseDeviceValidation { get; set; }

        /// <summary>Default registration options</summary>
        public static EntitlementRegistrationOptions Default = new EntitlementRegistrationOptions();
    }
}