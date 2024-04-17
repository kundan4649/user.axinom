using z5.ms.common;

namespace z5.ms.domain.subscription.payment
{
    /// <summary>Options for the PaymentModule (feature toggles etc.)</summary>
    public class PaymentRegistrationOptions : IComponentRegistrationOptions
    {
        /// <summary>Default registration options</summary>
        public static PaymentRegistrationOptions Default = new PaymentRegistrationOptions();

        // Integrate the adyen payment provider
        public bool EnableAdyenPayments { get; set; } = false;

        // Integrate the billdesk payment provider
        public bool EnableBilleskPayments { get; set; } = false;

        // Integrate the fortumo payment provider
        public bool EnableFortumoPayments { get; set; } = false;

        // Integrate google in app purchases
        public bool EnableGoogleInAppPurchases { get; set; } = false;

        // Use the mock google api (for testing environments)
        public bool UseMockGoogleApi { get; set; } = false;

        // Integrate apple in app purchases
        public bool EnableAppleInAppPurchases { get; set; } = false;

        // Use the mock apple api (for testing environments)
        public bool UseMockAppleApi { get; set; } = false;

        // Integrate the dialog payment provider
        public bool EnableDialogPayments { get; set; } = false;

        // Integrate the dummy payment provider for testing payments
        public bool EnableDummyPayments { get; set; } = false;

        /// <summary>Include data from subscribers table in internal api queries</summary>
        public bool EnableSubscriberData { get; set; } = true;
        
        /// <summary>Enable template controllers</summary>>
        public bool EnableTemplateControllers { get; set; } = true;
    }
}
