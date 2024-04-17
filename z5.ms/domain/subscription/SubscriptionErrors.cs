using System;
using z5.ms.common.abstractions;

namespace z5.ms.domain.subscription
{
    //TODO define actual error codes instead of '1'
    //TODO discuss if we should merge method/context specific errors together
    /// <summary>Errors used by Subscription Module </summary>
    public class SubscriptionErrors
    {
        /// <summary>Subscription plan is not available at current time. Used by v1 </summary>
        public Error PlanNotAvailable { get; } =
            new Error {Code = 3104, Message = "This subscription plan is not available at this time"};
        
        /// <summary>Subscription plan is not available at current time. Used by v2 </summary>
        public Error PlanNotAvailable2 { get; } =
            new Error {Code = 3131, Message = "This subscription plan is not available at this time"};

        /// <summary>Subscription plan is not available for given country </summary>
        public Error PlanNotAvailableForCountry { get; } =
            new Error { Code = 3122, Message = "This subscription plan is not available for current country" };

        /// <summary>A subscription cannot be made with this subscription plan. Used by v1 </summary>
        public Error CanNotSubscribeForPlan { get; } = new Error
        {
            Code = 3105,
            Message = "A subscription cannot be made with this subscription plan"
        };

        /// <summary>A subscription cannot be made with this subscription plan. Used by v2 </summary>
        public Error CanNotSubscribeForPlan2 { get; } = new Error
        {
            Code = 3132,
            Message = "A subscription cannot be made with this subscription plan"
        };
        
        /// <summary>Free trial periods are not available with specified payment provider </summary>
        public Error TrialNotAvailable { get; } = new Error
        {
            Code = 3106,
            Message = "Free trial periods are not available with this payment provider"
        };
        
        /// <summary>Couldn't find promotion code for specified subscription plan </summary>
        [Obsolete("Use PromotionCodeIsNotValid")]
        public Error InvalidPromotionCode { get; } = new Error {Code = 3107, Message = "Invalid promotion code"};
        
        /// <summary>Unsupported payment provider for specified subscription plan </summary>
        public Error UnsupportedPaymentProvider { get; } = new Error
        {
            Code = 3108,
            Message = "Unsupported payment provider for specified subscription plan"
        };
        
        /// <summary>User already has an active subscription and multiple subscriptions per user is disabled</summary>
        public Error AlreadySubscribed { get; } =
            new Error {Code = 3109, Message = "User already has an active subscription"};

        /// <summary>
        /// Multiple subscriptions are allowed and user already has an active subscription with this subscription plan
        /// </summary>
        public Error AlreadySubscribedWithPlan { get; } = new Error
        {
            Code = 3110,
            Message = "User already has an active subscription with this subscription plan"
        };
        
        /// <summary>Subscription preparation failed. Contains Db Error message </summary>
        public Error PrepareFailed { get; } = new Error {Code = 3111, Message = "Subscription preparation failed"};
        
        /// <summary>Couldn't find subscription by Id </summary>
        public Error NotFoundById { get; } = new Error {Code = 3112, Message = "Subscription couldn't be found"};

        /// <summary>Couldn't find subscription plan for specified Id </summary>
        public Error NoPlanForId { get; } = new Error {Code = 3113, Message = "Subscription plan couldn't be found"};

        //TODO maybe we can use ActivateFailed instead
        /// <summary>Subscription activation failed </summary>
        public Error ActivateError { get; } = new Error {Code = 3114, Message = "Subscription activation failed"};

        //TODO maybe we can use ActivateError instead
        /// <summary>Subscription activation failed  </summary>
        public Error ActivateFailed { get; } = new Error {Code = 3905, Message = "Subscription activation failed"};

        /// <summary>Failed to cancel subscription </summary>
        public Error CancelFailed { get; } = new Error {Code = 3116, Message = "Subscription cancelation failed"};

        //TODO Better naming or use one
        /// <summary>Subscription plan couldn't be found </summary>
        public Error PlanNotFound { get; } = new Error {Code = 3900, Message = "Subscription plan couldn't be found"};

        //TODO Better naming or use one
        /// <summary>Subscription plan couldn't be found </summary>
        public Error PlanNotFound2 { get; } = new Error {Code = 3344, Message = "Subscription plan couldn't be found"};

        /// <summary>Subscription not found </summary>
        public Error SubscriptionNotFound { get; } = new Error {Code = 1, Message = "Subscription not found"};

        /// <summary>This user has been deleted. </summary>
        public Error CrmUserDeleted { get; } = new Error {Code = 1, Message = "This user has been deleted."};

        /// <summary>User plan system and subscription plan system do not match </summary>
        public Error CrmUserAndSystemPlanMismatch { get; } = new Error
        {
            Code = 1,
            Message = "Subscription plan system does not match user's system."
        };

        /// <summary>Subscription plan is not supported for recurring payments </summary>
        public Error RecurringNotSupported { get; } =
            new Error {Code = 1, Message = "Unsupported subscription plan for recurring payments"};

        /// <summary>Payment provider is not supported for recurring payments </summary>
        public Error ProviderRecurringNotSupported { get; } =
            new Error {Code = 1, Message = "Unsupported payment provider for recurring payments"};

        /// <summary>IP address is not valid </summary>
        public Error InvalidIpAddress { get; } = new Error {Code = 3, Message = "IP address is not valid"};

        //TODO smelly. Not sure if we want to include exception info.
        /// <summary>Inserting subscriber failed. Exception info is included </summary>
        public Error InsertSubscriberFailed { get; } = new Error {Code = 1, Message = "Failed to insert subscriber"};

        /// <summary> Could not find subscriber. Also contains subscriber Id and service status code</summary>
        public Error SubscriberNotFound { get; } =
            new Error {Code = 1, Message = "Could not find subscriber in user service"};

        #region Devices

        /// <summary>Device already exists </summary>
        public Error DeviceExists { get; } = new Error {Code = 3600, Message = "Device already exists"};

        /// <summary>User has no active subscriptions</summary>
        public Error DeviceNoSubscriptions { get; } =
            new Error {Code = 3601, Message = "User doesn't have any active subscriptions"};

        /// <summary>Unable to add a new device, also provides max_devices and current_devices </summary>
        public Error CannotAddDevice { get; } = new Error {Code = 3602, Message = "Unable to add new device"};

        /// <summary>Insert operation failed </summary>
        public Error DeviceInsertFailed { get; } = new Error {Code = 3603, Message = "Insert operation failed"};

        /// <summary>Device not found </summary>
        public Error DeviceNotFound { get; } = new Error {Code = 3604, Message = "Device to update not found" };

        /// <summary>Device not found used by entitlements</summary>
        public Error DeviceNotFound2 { get; } = new Error { Code = 3608, Message = "Device not found" };

        /// <summary>Update operation failed </summary>
        public Error DeviceUpdateFailed { get; } = new Error {Code = 3605, Message = "Update operation failed"};

        /// <summary>Delete operation requires relogin </summary>
        public Error DeviceRelogRequired { get; } =
            new Error {Code = 3606, Message = "Delete operation requires relogin"};

        /// <summary>Removing devives is allowed only once in 24 hours </summary>
        public Error DeleteCooldownSurpassed { get; } =
            new Error {Code = 3607, Message = "Delete devices is allowed only once in 24 hours"};

        /// <summary>Max number of devices allowed by subscription plan exceeded </summary>
        public Error DeviceAllowanceExceeded { get; } = new Error { Code = 3820, Message = "Max number of devices exceeded" };

        #endregion

        #region Payment & Purchase

        /// <summary>Payment failed</summary>
        public Error PaymentFailed { get; } = new Error {Code = 3408, Message = "Payment failed"};

        /// <summary>Payment not found </summary>
        public Error PaymentNotFound { get; } = new Error {Code = 1, Message = "Payment not found"};
        
        /// <summary>Payment provider does not exist </summary>
        public Error NoPaymentProvider { get; } = new Error {Code = 3117, Message = "Payment provider does not exist"};
        
        /// <summary>Payment provider for donation does not exist </summary>
        public Error NoDonationPaymentProvider { get; } = new Error {Code = 1, Message = "Payment provider does not exist"};

        /// <summary>Payment provider does not exist when searching for purchase </summary>
        public Error NoPurchasePaymentProvider { get; } = new Error {Code = 3118, Message = "Payment provider does not exist"};

        /// <summary>Payment provider does not exist when searching for subscription </summary>
        public Error NoSubscriptionPaymentProvider { get; } =
            new Error {Code = 3119, Message = "Payment provider does not exist"};

        /// <summary>User already used this promotion</summary>
        public Error AlreadyUsedPromotion { get; } = new Error { Code = 3345, Message = "User already used this promotion" };

        /// <summary>User is not eligible to get this promotion</summary>
        public Error NotEligibleForPromotion { get; } = new Error { Code = 3346, Message = "User is not eligible to get this promotion" };

        /// <summary>Puchase not found </summary>
        public Error PurchaseNotFound { get; } = new Error {Code = 1, Message = "Purchase not found"};

        /// <summary>Payment Plan could not be found </summary>
        public Error PaymentPlanNotFound { get; } = new Error {Code = 1, Message = "Payment plan couldn't be found"};

        /// <summary>Unsupported payment provider for specified purchase plan </summary>
        public Error PurchaseUnsupportedPaymentProvider { get; } = new Error{ Code = 3202, Message = "Unsupported payment provider for specified purchase plan" };

        /// <summary>Invalid promo code for this purchase plan </summary>
        public Error PurchaseInvalidPromotionCode { get; } =
            new Error {Code = 1, Message = "Invalid promo code for this purchase plan"};

        /// <summary>This purchase plan is not available at this time </summary>
        public Error PurchasePlanNotAvailable { get; } =
            new Error {Code = 1, Message = "This purchase plan is not available at this time"};

        /// <summary>This asset cannot be purchased with this purchase plan </summary>
        public Error PurchaseAssetDisabled { get; } = new Error { Code = 3201, Message = "This asset cannot be purchased with this purchase plan" };

        /// <summary>This asset cannot be purchased with this purchase plan </summary>
        public Error PurchaseAssetNotFound { get; } = new Error{ Code = 3203, Message = "Asset not found" };

        /// <summary>Purchase preparation failed </summary>
        public Error PurchasePrepareFailed { get; } = new Error {Code = 3204, Message = "Purchase preparation failed"};

        /// <summary>Purchase activation has failed </summary>
        public Error PurchaseActivationFailed { get; } = new Error {Code = 3205, Message = "Purchase activation failed"};

        /// <summary>User already has an active purchase</summary>
        public Error AlreadyPurchased { get; } = new Error { Code = 3206, Message = "Asset has already been purchased" };

        /// <summary>Record payment failed. Contains error message </summary>
        public Error RecordPaymentFailed { get; } = new Error {Code = 3114, Message = "Record payment failed"};

        /// <summary>Record receipt failed. Contains error message </summary>
        public Error ReceiptFailed { get; } = new Error{Code = 3115, Message = "Record receipt failed"};

        /// <summary>Payment has already been used to activate a subscription / purchase</summary>
        public Error DuplicatePayment { get; set; } = new Error
        {
            Code = 3116,
            Message = "This payment has already been used"
        };

        #endregion

        #region Donation
        
        /// <summary>Donation not found </summary>
        public Error DonationNotFound { get; } = new Error {Code = 1, Message = "Donation not found"};

        /// <summary>Donation plan couldn't be found </summary>
        public Error DonationPlanNotFound { get; } = new Error {Code = 1, Message = "Donation plan couldn't be found"};

        /// <summary>A donation cannot be made with this subscription plan </summary>
        public Error DonationsDisabled { get; } =
            new Error {Code = 1, Message = "A donation cannot be made with this subscription plan"};

        /// <summary>This donation plan is not available at this time </summary>
        public Error DonationUnavailable { get; } = new Error{ Code = 1, Message = "This donation plan is not available at this time"};

        /// <summary>Unsupported payment provider for specified donation plan </summary>
        public Error DonationProviderUnsupported { get; } = new Error
        {
            Code = 1,
            Message = "Unsupported payment provider for specified donation plan"
        };

        /// <summary>Donation preparation failed </summary>
        public Error DonationPrepareFailed { get; } = new Error {Code = 1, Message = "Donation preparation failed"};

        /// <summary>Donation activation failed </summary>
        public Error DonationActivationFailed { get; } = new Error{Code = 1, Message = "Donation activation failed"};

        #endregion

        #region Asset Repository
        
        /// <summary>Asset type is unsupported in AssetTitleRepository. Contains asset Id </summary>
        public Error AssetUnsupported { get; } = new Error{Code = 1, Message = "Asset type is unsupported in AssetTitleRepository"};

        /// <summary>Error fetching asset. Contains url, status code, response body </summary>
        public Error AssetFetch { get; } = new Error {Code = 1, Message = "Error fetching asset"};

        #endregion

        #region Fortumo

        /// <summary>Subscription couldn't be found when acivating recurring subscription </summary>
        public Error FortumoRecurringNotFound { get; } =
            new Error {Code = 3400, Message = "Subscription couldn't be found"};

        /// <summary>Invalid signature when activating recurring subscription </summary>
        public Error FortumoRecurringInvalidSignature { get; } = new Error {Code = 3401, Message = "Invalid Signature"};

        /// <summary>Subscription activation failed when </summary>
        public Error FortumoActivationFailed { get; } = new Error
        {
            Code = 3402,
            Message = "Subscription activation failed (Db operation failure)"
        };
        
        /// <summary>Subscription couldn't be found when activating subscription </summary>
        public Error FortumoNotFound { get; } = new Error {Code = 3403, Message = "Subscription couldn't be found"};

        /// <summary>Invalid signature when activating subscription </summary>
        public Error FortumoInvalidSignature { get; } = new Error {Code = 3404, Message = "Invalid signature"};

        /// <summary>Subscription couldn't be found when canceling subscription </summary>
        public Error FortumoCancelNotFound { get; } =
            new Error {Code = 3405, Message = "Subscription couldn't be found"};

        /// <summary>Subscription cancelation failed </summary>
        public Error FortumoCancelFailed { get; } = new Error{Code = 3406, Message = "Subscription cancelation failed"};

        /// <summary>Subscription couldn't be found for specified subscription Id </summary>
        public Error FortumoGetIdNotFound { get; } = new Error {Code = 3407, Message = "Subscription couldn't be found"};
        
        #endregion

        #region Billdesk

        /// <summary>Subscription plan couldn't be found </summary>
        public Error BilldeskPlanNotFound { get; } = new Error {Code = 3301, Message = "Subscription plan couldn't be found"};

        /// <summary>Billdesk platform error. Contains error log </summary>
        public Error BilldeskPlatformError { get; } = new Error {Code = 3302, Message = "Billdesk platform error"};

        /// <summary>Invalid Signature </summary>
        public Error BilldeskInvalidSignature { get; } = new Error {Code = 3303, Message = "Invalid signature"};

        /// <summary>Subscription couldn't be found </summary>
        public Error BilldeskNotFound { get; } = new Error {Code = 3305, Message = "Subscription couldn't be found"};

        #endregion

        #region Mife

        /// <summary>Mife platform error. Contains error log </summary>
        public Error MifePlatformError { get; } = new Error { Code = 3351, Message = "Mife platform error" };

        /// <summary>Mife operator name is not defined in configuration</summary>
        public Error MifeOperatorNotSupported { get; } = new Error { Code = 3352, Message = "Mife operator not supported" };

        /// <summary>Activating app subscription with web callback</summary>
        public Error MifeAppPaymentWithWebCallback { get; } = new Error { Code = 3353, Message = "Activating app payment with web callback" };

        /// <summary>Activating web subscription with app callback</summary>
        public Error MifeWebPaymentWithAppCallback { get; } = new Error { Code = 3354, Message = "Activating web payment with app callback" };

        /// <summary>Subscription cancellation token not found</summary>
        public Error MifeSubscriptionCancellationTokenNotFound { get; } = new Error { Code = 3355, Message = "Subscription cancellation token not found" };

        /// <summary>Mife operator does not support TVOD payments</summary>
        public Error MifeOperatorDoesNotSupportTvod { get; } = new Error { Code = 3356, Message = "Mife operator does not support TVOD payments" };
        
        /// <summary>Mife subscription not found</summary>
        public Error MifeNotFound{ get; } = new Error { Code = 3357, Message = "Subscription couldn't be found" };

        /// <summary>Mife purchase couldn't be found</summary>
        public Error MifePurchaseNotFound { get; } = new Error {Code = 3358, Message = "Purchase couldn't be found"};

        
        #endregion


        #region Promotions

        /// <summary>Subscription couldn't be found </summary>
        public Error PromotionCodeIsNotValid { get; } = new Error { Code = 3340, Message = "The promotion code is not valid" };

        /// <summary>Subscription plan couldn't be found </summary>
        public Error PromotionCodeIsNotYetValid { get; } = new Error { Code = 3341, Message = "The promotion code is not valid yet" };

        /// <summary>Billdesk platform error. Contains error log </summary>
        public Error PromotionCodeisNoLongerActive { get; } = new Error { Code = 3342, Message = "The promotion code is not valid anymore" };

        /// <summary>Invalid Signature </summary>
        public Error PromotionsAreNotSupportedByThisPaymentProvider { get; } = new Error { Code = 3343, Message = "Promotions are not supported for this payment provider" };
        
        #endregion


        #region Adyen
        
        /// <summary>Subscription couldn't be found </summary>
        public Error AdyenNotFound { get; } = new Error{Code = 3700, Message = "Subscription couldn't be found"};

        /// <summary>Invalid signature. Used in Activate subscription </summary> 
        public Error AdyenInvalidSignatureSubscription { get; } = new Error {Code = 3701, Message = "Invalid signature"};

        /// <summary>Purchase couldn't be found </summary>
        public Error AdyenPurchaseNotFound { get; } = new Error {Code = 3702, Message = "Purchase couldn't be found"};

        /// <summary>Invalid signature. Used in Activate purchase </summary> 
        public Error AdyenInvalidSignaturePurchase { get; } = new Error {Code = 3703, Message = "Invalid signature"};

        /// <summary>Purchase couldn't be found </summary>
        public Error AdyenDonationNotFound { get; } = new Error {Code = 3704, Message = "Donation couldn't be found"};

        /// <summary>Invalid signature. Used in Activate donation </summary>
        public Error AdyenInvalidSignatureDonation { get; } = new Error {Code = 3705, Message = "Invalid signature"};

        /// <summary>Adyen secret key is missing from subscription plan. Error is from Subscription token generating</summary>
        public Error AdyenKeyMissingSubscription { get; } =
            new Error {Code = 3706, Message = "Adyen secret key is missing from subscription plan."};

        /// <summary> Adyen secret key is missing from subscription plan. Error is from Purchase token generating</summary>
        public Error AdyenKeyMissingPurchase { get; } =
            new Error {Code = 3707, Message = "Adyen secret key is missing from subscription plan"};

        /// <summary>Adyen secret key is missing from subscription plan. Error is from Donation token generating</summary>
        public Error AdyenKeyMissingDonation { get; } =
            new Error {Code = 3708, Message = "Adyen secret key is missing from subscription plan"};

        /// <summary>Adyen cancel for subscription failed. Contains subscription, status code and response content</summary>
        public Error AdyenCancelFailed { get; } =
            new Error {Code = 3709, Message = "Adyen cancel for subscription failed"};

        /// <summary>For Adyen a registered email address is required for recurring subscription payments</summary>
        public Error AdyenEmailRequiredFailed { get; } =
            new Error { Code = 3710, Message = "A registered email address is required for recurring subscription payments" };
       
        #endregion

        #region Apple

        /// <summary> Error calling the Apple token verify API. Contains status code and token </summary>
        public Error AppleTokenVerifyFailed { get; } =
            new Error {Code = 3124, Message = "Error calling the Apple token verify API"};

        /// <summary>Timeout happened while calling Apple in-app purchase validation </summary>
        public Error AppleTimeout { get; } = new Error
        {
            Code = 3125,
            Message = "Timeout happened while calling Apple in-app purchase validation"
        };
        
        /// <summary> Apple payment subscription couldn't be found </summary>
        public Error AppleNotFound { get; } = new Error {Code = 3126, Message = "Subscription couldn't be found"};

        /// <summary>Apple payment incorrect payment provider </summary>
        public Error AppleIncorrectProvider { get; } = new Error {Code = 3127, Message = "Incorrect payment provider"};

        /// <summary>Could not find correct receipt info. Contains receipt </summary>
        public Error AppleReceiptNotFound { get; } =
            new Error {Code = 3128, Message = "Could not find correct receipt info"};

        /// <summary>Apple payment subscription expired. Contains receipt </summary>
        public Error AppleExpired { get; } = new Error { Code = 3129, Message = "Subscription expired"};

        /// <summary> Apple payment subscription plan is not matched with provided receipt </summary>
        public Error ApplePlanReceiptMismatch { get; } = new Error
        {
            Code = 3130,
            Message = "Subscription plan is not matched with provided receipt"
        };
        #endregion

        #region Google

        /// <summary>Google payment subscription couldn't be found </summary>
        public Error GoogleNotFound { get; } = new Error {Code = 3140, Message = "Subscription couldn't be found"};

        /// <summary>Google payment incorrect payment provider </summary>
        public Error GoogleIncorrectProvider { get; } = new Error {Code = 3141, Message = "Incorrect payment provider"};
        
        /// <summary> Google payment subscription plan is not matched with provided receipt </summary>
        public Error GooglePlanReceiptMismatch { get; } = new Error
        {
            Code = 3142,
            Message = "Subscription plan is not matched with provided receipt"
        };

        /// <summary>Subscription couldn't be found for canceling subscription in google service </summary>
        public Error GoogleCancelNotFound { get; } =
            new Error {Code = 3143, Message = "Subscription couldn't be found"};

        /// <summary>
        /// No purchase token found associated with specified subscription for canceling subsciption in google service 
        /// </summary>
        public Error GoogleCancelTokenNotFound { get; } = new Error
        {
            Code = 3144,
            Message = "No purchase token found associated with specified subscription"
        };

        #endregion

        #region Refunds

        /// <summary>Requested amount exceeds the allowed refund amount</summary>
        public Error RefundAmountExceeded { get; } = new Error { Code = 3901, Message = "Requested amount exceeds the allowed refund amount" };

        /// <summary>Invalid refund action</summary>
        public Error InvalidRefundAction { get; } = new Error { Code = 3902, Message = "Invalid refund action" };

        #endregion
    }
}