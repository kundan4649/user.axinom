using System;
using z5.ms.common;

namespace z5.ms.domain.subscription.entitlement.config
{
    /// <summary>Catalog BC registration options</summary>
    [Obsolete("Use entitlement2")]
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
        
        /// <summary>Use the Google Playstore subscription entitlement logic</summary>
        public bool UseGoogleEntitlement { get; set; }
        
        /// <summary>Use the Apple App Store subscription entitlement logic</summary>
        public bool UseAppleEntitlement { get; set; }

        /// <summary>Use the Dummy entitlement logic</summary>
        public bool UseDummyEntitlement { get; set; }

        /// <summary>Use BuyDRM DRM token generation</summary>
        public bool UseBuyDrm { get; set; }
        
        /// <summary>Use AxinomDRM DRM token generation</summary>
        public bool UseAxinomDrm{ get; set; }
        
        /// <summary>Use Verizon Edgecast CDN token generation</summary>
        public bool UseEdgecastCdn { get; set; }

        /// <summary>Default registration options</summary>
        public static EntitlementRegistrationOptions Default = new EntitlementRegistrationOptions();
    }
}