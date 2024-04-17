namespace z5.ms.infrastructure.user.identity
{
    /// <summary>Custom claims for Z5</summary>
    public class Z5Claims
    {
        public const string UserId = "user_id";
        public const string System = "system";
        public const string UserEmail = "user_email";
        public const string UserEmailNotVerified = "user_email_not_verified";
        public const string UserMobile = "user_mobile";
        public const string UserMobileNotVerified = "user_mobile_not_verified";
        public const string ActivationDate = "activation_date";
        public const string CreatedDate = "created_date";
        public const string RegistrationCountry = "registration_country";
        public const string Subscriptions = "subscriptions";
        public const string CurrentCountry = "current_country";
        public const string DeviceId = "device_id";
    }
}