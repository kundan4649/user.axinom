using z5.ms.common.abstractions;

namespace z5.ms.domain.entitlement4
{    
    //TODO define actual error codes instead of '1'
    /// <summary>Errors used by Entitlement Module</summary>
    public class EntitlementErrors
    {
        /// <summary>User authentication token is invalid </summary>
        public Error InvalidToken { get; } = new Error {Code = 1, Message = "User authentication token is invalid"};

        /// <summary>Invalid secret token </summary>
        public Error CmsInvalidToken { get; } = new Error {Code = 1, Message = "Invalid secret token"};
        
        /// <summary>Unsupported asset type </summary>
        public Error NotSupported { get; } = new Error {Code = 1, Message = "Unsupported asset type"};

        /// <summary>Asset for Id not found </summary>
        public Error AssetNotFound { get; } = new Error {Code = 1, Message = "Couldn't find asset for id"};
        
        /// <summary>Invalid DRM key </summary>
        public Error InvalidDrm { get; } = new Error {Code = 3800, Message = "Invalid DRM key"};

        /// <summary>This endpoint can only be used for drm enabled content </summary>
        public Error DrmOnly { get; } =
            new Error {Code = 3801, Message = "This endpoint can only be used for drm enabled content"};

        /// <summary>License is expired </summary>
        public Error LicenseExpired { get; } = new Error {Code = 3802, Message = "License is expired"};

        /// <summary>License is expired </summary>
        public Error InvalidCountry { get; } = new Error { Code = 3803, Message = "Content is not available for this country" };

        /// <summary>Subscription couldn't be found </summary>
        public Error NotFound { get; } = new Error {Code = 3804, Message = "Subscription couldn't be found"};

        //TODO maybe we could use NotFound instead
        /// <summary>Subscription or purchase couldn't be found </summary>
        public Error PurchaseNotFound { get; } = new Error {Code = 3804, Message = "Subscription or purchase couldn't be found"};
        
        /// <summary>Subscription is expired </summary>
        public Error Expired { get; } = new Error {Code = 3805, Message = "Subscription is expired"};
        
        /// <summary>Subscription plan couldn't be found</summary>
        public Error PlanNotFound { get; } = new Error {Code = 3806, Message = "Subscription plan couldn't be found"};

        /// <summary>Subscription plan doesn't contain specified asset type or asset id </summary>
        public Error PlanNotContain { get; } = new Error
        {
            Code = 3807,
            Message = "Subscription plan doesn't contain specified asset type or asset id"
        };

        /// <summary>Error calling the Google authorization API. Contains status code </summary>
        public Error GoogleAuthorization { get; } =
            new Error {Code = 3808, Message = "Error calling the Google authorization API"};
        
        /// <summary>Could not create new Google authorization token. Contains response body </summary>
        public Error GoogleAuthorizationToken { get; } =
            new Error {Code = 3809, Message = "Could not create new Google authorization token"};

        /// <summary>Error calling the Google store API. Contains status code </summary>
        public Error GoogleStore { get; } = new Error {Code = 3810, Message = "Error calling the Google store API"};

        /// <summary>No valid google subscription for token. Contains token, response body </summary>
        public Error GoogleNotValidToken { get; } =
            new Error {Code = 3811, Message = "No valid google subscription for token"};

        /// <summary>Subscription is not valid for token. Contains token, response body </summary>
        public Error GoogleTokenInvalid { get; } =
            new Error {Code = 3812, Message = "Subscription is not valid for token"};
        
        /// <summary>Timeout when calling for address. Contains client address </summary>
        public Error Timeout { get; } = new Error {Code = 3813, Message = "Timeout when requesting posting request" };

        /// <summary>Task cancelled when posting request. Contains client address </summary>
        public Error TaskCancel { get; } = new Error {Code = 3814, Message = "Task cancelled when posting request"};

        /// <summary>Error calling the Apple token verify API. Contains status code and response body </summary>
        public Error AppleTokenVerify { get; } =
            new Error {Code = 3815, Message = "Error calling the Apple token verify API"};

        /// <summary>Could not find a correct receipt info. Contains response body </summary>
        public Error ReceiptIncorrect { get; } =
            new Error {Code = 3816, Message = "Could not find a correct receipt info"};
    }
}