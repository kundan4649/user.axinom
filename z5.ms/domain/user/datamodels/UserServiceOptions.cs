using System;
using System.Collections.Generic;
using System.Linq;
using z5.ms.common.extensions;
using z5.ms.domain.user.viewmodels;

namespace z5.ms.domain.user.datamodels
{
    /// <summary>
    /// Configuration options for the user service
    /// </summary>
    public class UserServiceOptions
    {
        private string _googleAuthApiUrl;
        private string _facebookAuthApiUrl;
        private string _googleAuthApiRedirectUrl;
        private string _subscriptionApiUrl;
        private string _idServiceBaseUrl;
        private string _frontEndUrl;
        private string _googleAuthApiTokenUrl;
        private string _catalogApiUrl;

        /// <summary>Default value for a user's system type</summary>
        public string DefaultSystemType { get; set; } = "Internal";
        
        /// <summary>Key for signing user authentication tokens</summary>
        public string SessionKey { get; set; }

        /// <summary>The URL of the subscription web page</summary>
        public string FrontEndUrl
        {
            get => _frontEndUrl;
            set => _frontEndUrl = value?.TrimEnd('/');
        }

        /// <summary>The URL of the catalog api</summary>
        public string CatalogApiUrl
        {
            get => _catalogApiUrl;
            set => _catalogApiUrl = value?.TrimEnd('/');
        }

        /// <summary>Sms service host url</summary>
        public string SmsHost { get; set; }

        /// <summary>Mail server host name</summary>
        public string MailHost { get; set; }

        /// <summary>Mail server port</summary>
        public int MailPort { get; set; }

        /// <summary>Mail user name to send notification mails</summary>
        public string MailUserName { get; set; }

        /// <summary>Mail password to send notification mails</summary>
        public string MailPassword { get; set; }

        /// <summary>User service database connection string</summary>
        public string MsDatabaseConnection { get; set; }

        /// <summary>Secret for internal api authentication</summary>
        public string InternalApiSecret { get; set; }

        /// <summary>Consumer key provided from twitter to make authenticated requests</summary>
        public string TwitterConsumerKey { get; set; }

        /// <summary>Consumer secret provided from twitter to make authenticated requests</summary>
        public string TwitterConsumerSecret { get; set; }

        /// <summary>Url to get access token using code response of login form from google authentication service </summary>
        public string GoogleAuthApiTokenUrl
        {
            get => _googleAuthApiTokenUrl;
            set => _googleAuthApiTokenUrl = value?.TrimEnd('/');
        }

        /// <summary>Request body to get access token using code response of login form from google authentication service </summary>
        public string GoogleAuthApiTokenRequestBody { get; set; }

        /// <summary>Url to get user profile information from google authentication service </summary>
        public string GoogleAuthApiUrl
        {
            get => _googleAuthApiUrl;
            set => _googleAuthApiUrl = value?.TrimEnd('/');
        }

        /// <summary>Url to get user profile information from facebook authentication service </summary>
        public string FacebookAuthApiUrl
        {
            get => _facebookAuthApiUrl;
            set => _facebookAuthApiUrl = value?.TrimEnd('/');
        }

        /// <summary>Configured client id from google developer console</summary>
        public string GoogleAuthApiClientId { get; set; }

        /// <summary>Configured client secret from google developer console</summary>
        public string GoogleAuthApiClientSecret { get; set; }

        /// <summary>Configured redirect url from google developer console</summary>
        public string GoogleAuthApiRedirectUrl
        {
            get => _googleAuthApiRedirectUrl;
            set => _googleAuthApiRedirectUrl = value?.TrimEnd('/');
        }

        /// <summary>Url of subscription service api</summary>
        public string SubscriptionApiUrl
        {
            get => _subscriptionApiUrl;
            set => _subscriptionApiUrl = value?.TrimEnd('/');
        }

        /// <summary>Url of identity service api</summary>
        public string IdServiceBaseUrl
        {
            get => _idServiceBaseUrl;
            set => _idServiceBaseUrl = value?.TrimEnd('/');
        }

        /// <summary>Number of characters in numeric mobile confirmation key sent on registration</summary>
        public int MobileConfirmationKeyLength { get; set; }

        /// <summary>Expiry time in minutes for mobile confirmation key sent on registration</summary>
        public int MobileConfirmationKeyExpiry { get; set; }

        /// <summary>Number of characters in numeric mobile confirmation key sent on password reset</summary>
        public int MobilePasswordResetKeyLength { get; set; }

        /// <summary>Expiry time in minutes for mobile confirmation key sent on password reset</summary>
        public int MobilePasswordResetKeyExpiry { get; set; }

        /// <summary>Expiry time in hours for email confirmation key sent on registration</summary>
        public int EmailConfirmationKeyExpiry { get; set; } = 24;

        /// <summary>
        /// A flag for determining the email verification step. If true, skip verification for all countries
        /// except the ones in <see cref="SkipVerifyEmailExceptInCountries"/>.
        /// </summary>
        public bool SkipVerifyEmail { get; set; } = true;

        /// <summary>Users from these countries listed still need to verify email before logging in.</summary>
        public List<string> SkipVerifyEmailExceptInCountries { get; set; } = new List<string>();

        /// <summary>
        /// A flag for determining the mobile verification step. If true, skip verification for all countries
        /// except the ones in <see cref="SkipVerifyMobileExceptInCountries"/>.
        /// </summary>
        public bool SkipVerifyMobile { get; set; } = true;

        /// <summary>Users from these countries listed still need to verify mobile before logging in.</summary>
        public List<string> SkipVerifyMobileExceptInCountries { get; set; } = new List<string>() { "IN"};

        /// <summary>Check if the verification can be skipped for authentication <paramref name="type"/> in <paramref name="country"/></summary>
        public bool SkipVerify(AuthenticationMethod type, string country)
        {
            switch (type)
            {
                case AuthenticationMethod.Email:
                    return SkipVerifyEmail && !SkipVerifyEmailExceptInCountries.Any(c => c.EqualsIgnoreCase(country));
                case AuthenticationMethod.Mobile:
                    return SkipVerifyMobile && !SkipVerifyMobileExceptInCountries.Any(c => c.EqualsIgnoreCase(country));
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, 
                        $"Authentication method {type} cannot be used to determine if verification should be skipped");
            }
        }
        public int SubscriptionRequestTimeout { get; set; }
        public string LaunchAPIUrl { get; set; }
    }
}
