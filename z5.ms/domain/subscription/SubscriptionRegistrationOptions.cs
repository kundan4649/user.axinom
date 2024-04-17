using z5.ms.common;

namespace z5.ms.domain.subscription
{
    /// <summary>
    /// Options for the SubscriptionModule (feature toggles etc.)
    /// </summary>
    public class SubscriptionRegistrationOptions : IComponentRegistrationOptions
    {
        /// <summary>Default registration options</summary>
        public static SubscriptionRegistrationOptions Default = new SubscriptionRegistrationOptions();

        /// <summary>Enable functionality to buy and manage content subscriptions</summary>
        public bool EnableSubscriptions { get; set; } = true; // TODO: remove default when explicitly defined in DC

        /// <summary>Enable functionality to buy and manage content purchases and rental</summary>
        public bool EnablePurchases { get; set; } = true; // TODO: remove default when explicitly defined in DC

        /// <summary>Enable functionality to pay and manage donations</summary>
        public bool EnableDonations { get; set; } = true; // TODO: remove default when explicitly defined in DC

        /// <summary>Send notifications on subscription activation</summary>
        public bool SubscriptionActivationNotifications { get; set; } = true;

        /// <summary>Send notifications on purchase activation</summary>
        public bool PurchaseActivationNotifications { get; set; } = true;

        /// <summary>Enable CRM features to create / update subscriptions and purchases</summary>
        public bool EnableCrmFeatures { get; set; } = false;

        /// <summary>Enable device management features</summary>
        public bool EnableDeviceManagement { get; set; } = false;

        /// <summary>Include data from subscribers table in internal api queries</summary>
        public bool EnableSubscriberData { get; set; } = true;
        
        //TODO Need to organize into better options
        /// <summary>Enable template controllers</summary>>
        public bool EnableTemplateControllers { get; set; } = true;
    }
}
