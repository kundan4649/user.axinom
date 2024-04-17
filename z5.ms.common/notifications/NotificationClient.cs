using System;
using Amazon.S3;
using Microsoft.Extensions.Options;

namespace z5.ms.common.notifications
{
    /// <summary>Notification service to send sms and email to user for notifications</summary>
    public interface INotificationClient
    {
        /// <summary>Send a custom notification</summary>
        void SendCustom(Notification notification, string country = null);

        /// <summary>Send an account activation email to user</summary>
        void SendRegistrationActivationEmail(string email, string emailConfirmationKey, string country);

        /// <summary>Send a password reset email to user</summary>
        void SendPasswordResetEmail(string email, string passwordResetKey, string country);

        /// <summary>Send a password recreation confirmation email to user</summary>
        void SendPasswordRecreationConfirmationEmail(string email, string country);

        /// <summary>Send an email to confirm new email</summary>
        void SendEmailAddEmail(string email, string emailConfirmationKey, string country);

        /// <summary>Send an email to warn user about new email</summary>
        void SendEmailAddSms(string mobile, string newEmail, string country);

        /// <summary>Send an email to confirm new email</summary>
        void SendMobileAddEmail(string email, string newMobile, string country);

        /// <summary>Send an email to warn user about new email</summary>
        void SendMobileAddSms(string mobile, string mobileConfirmationKey, string country);

        /// <summary>Send an sms to confirm mobile change</summary>
        void SendMobileChangeSms(string sms, string mobileChangeConfirmationKey, string country);

        /// <summary>Send an email to confirm successful registration</summary>
        void SendRegistrationConfirmationEmail(string email, string country);

        /// <summary>Send an account activation sms to user</summary>
        void SendRegistrationActivationSms(string mobile, string mobileConfirmationKey, string country);

        /// <summary>Send a password reset sms to user</summary>
        void SendPasswordResetSms(string mobile, string passwordResetKey, string country);

        /// <summary>Send a password recreation confirmation sms to user</summary>
        void SendPasswordRecreationConfirmationSms(string mobile, string country);
        
        /// <summary>Send an email warning that subscription payment failed</summary>
        void SendSubscriptionBillingWarningEmail(string email, string subscriptionPlanTitle, string paymentAmount, string country);

        /// <summary>Send an email warning that subscription has expired</summary>
        void SendSubscriptionExpiredEmail(string email, string subscriptionPlanTitle, string paymentAmount, string country);

        /// <summary>Send an email confirming subscription cancellation by user</summary>
        void SendSubscriptionCancellationEmail(string email, string subscriptionPlanTitle, string subscriptionEndDate, string country);

        /// <summary>Send an email confirming subscription activation</summary>
        void SendSubscriptionConfirmationEmail(string email, string subscriptionPlanTitle, DateTime? subscriptionEnd, string country, string promotionalText);

        /// <summary>Send an email confirming subscription renewal</summary>
        void SendSubscriptionRenewalEmail(string email, string subscriptionPlanTitle, DateTime? subscriptionEnd, string country, string promotionalText);

        /// <summary>Send an email regarding subscription failure</summary>
        void SendSubscriptionFailureEmail(string email, string subscriptionPlanTitle, string country);

        /// <summary>Send an sms confirming subscription activation</summary>
        void SendSubscriptionConfirmationSms(string mobile, string subscriptionPlanTitle, DateTime? subscriptionEnd, string country, string promotionalText);

        /// <summary>Send an sms confirming subscription renewal</summary>
        void SendSubscriptionRenewalSms(string mobile, string subscriptionPlanTitle, DateTime? subscriptionEnd, string country, string promotionalText, string subscriptionPlanDuration);

        /// <summary>Send an sms regarding subscription failure</summary>
        void SendSubscriptionFailureSms(string mobile, string subscriptionPlanTitle, string country);

        /// <summary>Send an sms notifying the user of subscription renewal info</summary>
        void SendRecurringSubscriptionRenewalInfoSms(string mobile, string subscriptionPlanTitle, DateTime? subscriptionEnd, string country, string subscriptionPlanDuration);
               
        /// <summary>Send an sms notifying the user of subscription end info</summary>
        void SendNonRecurringSubscriptionRenewalInfoSms(string mobile, string subscriptionPlanTitle, DateTime? subscriptionEnd, string country, string subscriptionPlanDuration);
        
        /// <summary>Send an email confirming purchase activation</summary>
        /// <returns></returns>
        void SendPurchaseConfirmationEmail(string email, string assetTitle, string country, string promotionalText);

        /// <summary>Send an email notifying of a failed pending payment</summary>
        void SendPendingPaymentFailureEmail(string email, string subscriptionPlanTitle, string paymentAmount, string country);

        /// <summary>Send an sms notifying the user that his/her subscription is expiring soon/has already expired</summary>
        void SendSubscriptionExpiryNotificationSms(string mobile, string subscriptionPlanTitle, DateTime? subscriptionEnd, string country, SubscriptionExpiryNotificationDay day);
    }

    /// <inheritdoc />
    public class NotificationClient : INotificationClient
    {
        private readonly IAWSNotificationQueue _notificationQueue;
        private readonly INotificationCountryOptionsProvider _countryOptionsProvider;
        private readonly IAmazonS3 _amazonS3Client;

        /// <summary>Constructor method for Notification Service</summary>
        /// <param name="notificationQueue"></param>
        /// <param name="options"></param>
        public NotificationClient(IAWSNotificationQueue notificationQueue, IOptions<NotificationOptions> options, IAmazonS3 amazonS3Client)
        {
            _notificationQueue = notificationQueue;
            _amazonS3Client = amazonS3Client;
            _countryOptionsProvider = new NotificationCountryOptionsProvider(options, _amazonS3Client);
        }

        /// <inheritdoc />
        public void SendCustom(Notification notification, string country = null)
        {
            if (country != null) notification.CountryOptions = GetCountryOptions(country);
            _notificationQueue.Send(notification);
        }

        /// <inheritdoc />
        public void SendRegistrationActivationEmail(string email, string emailConfirmationKey, string country)
            => _notificationQueue.Send(new Notification
            {
                Type = NotificationType.Email,
                To = email,
                TemplateName = "EmailTemplate_RegistrationActivation",
                CountryOptions = GetCountryOptions(country),
                Substitutions = new[]
                    {
                        new Substitution("[PLACEHOLDER_EmailConfirmationKey]", emailConfirmationKey)
                    }
            });

        /// <inheritdoc />
        public void SendPasswordResetEmail(string email, string passwordResetKey, string country)
            => _notificationQueue.Send(new Notification
            {
                Type = NotificationType.Email,
                To = email,
                TemplateName = "EmailTemplate_PasswordReset",
                CountryOptions = GetCountryOptions(country),
                Substitutions = new[]
                    {
                        new Substitution("[PLACEHOLDER_PasswordResetKey]", passwordResetKey)
                    }
            });

        /// <inheritdoc />
        public void SendPasswordRecreationConfirmationEmail(string email, string country)
            => _notificationQueue.Send(new Notification
            {
                Type = NotificationType.Email,
                To = email,
                TemplateName = "EmailTemplate_PasswordRecreationConfirmation",
                CountryOptions = GetCountryOptions(country),
                Substitutions = new[]
                {
                    new Substitution("[PLACEHOLDER_UserId]", email)
                }
            });

        /// <inheritdoc />
        public void SendEmailAddEmail(string email, string emailConfirmationKey, string country)
            => _notificationQueue.Send(new Notification
            {
                Type = NotificationType.Email,
                To = email,
                TemplateName = "EmailTemplate_AddEmail",
                CountryOptions = GetCountryOptions(country),
                Substitutions = new[]
                {
                    new Substitution("[PLACEHOLDER_EmailConfirmationKey]", emailConfirmationKey)
                }
            });

        /// <inheritdoc />
        public void SendEmailAddSms(string mobile, string newEmail, string country)
            => _notificationQueue.Send(new Notification
            {
                Type = NotificationType.Sms,
                To = mobile,
                TemplateName = "SmsTemplate_AddEmail",
                CountryOptions = GetCountryOptions(country),
                Substitutions = new[]
                {
                    new Substitution("[PLACEHOLDER_NewEmail]", newEmail)
                }
            });

        /// <inheritdoc />
        public void SendMobileAddEmail(string email, string newMobile, string country)
            => _notificationQueue.Send(new Notification
            {
                Type = NotificationType.Email,
                To = email,
                TemplateName = "EmailTemplate_AddMobile",
                CountryOptions = GetCountryOptions(country),
                Substitutions = new[]
                {
                    new Substitution("[PLACEHOLDER_NewMobile]", newMobile)
                }
            });

        /// <inheritdoc />
        public void SendMobileAddSms(string mobile, string mobileConfirmationKey, string country)
            => _notificationQueue.Send(new Notification
            {
                Type = NotificationType.Sms,
                To = mobile,
                TemplateName = "SmsTemplate_AddMobile",
                CountryOptions = GetCountryOptions(country),
                Substitutions = new[]
                {
                    new Substitution("[PLACEHOLDER_MobileConfirmationKey]", mobileConfirmationKey)
                }
            });

        /// <inheritdoc />
        public void SendMobileChangeSms(string mobile, string mobileChangeConfirmationKey, string country)
            => _notificationQueue.Send(new Notification
            {
                Type = NotificationType.Sms,
                To = mobile,
                TemplateName = "SmsTemplate_ChangeMobile",
                CountryOptions = GetCountryOptions(country),
                Substitutions = new[]
                {
                    new Substitution("[PLACEHOLDER_MobileChangeConfirmationKey]", mobileChangeConfirmationKey)
                }
            });

        /// <inheritdoc />
        public void SendRegistrationConfirmationEmail(string email, string country)
            => _notificationQueue.Send(new Notification
            {
                Type = NotificationType.Email,
                To = email,
                TemplateName = "EmailTemplate_RegistrationConfirmation",
                CountryOptions = GetCountryOptions(country),
                Substitutions = new Substitution[]
                    {
                    }
            });

        /// <inheritdoc />
        public void SendRegistrationActivationSms(string mobile, string mobileConfirmationKey, string country)
            => _notificationQueue.Send(new Notification
            {
                Type = NotificationType.Sms,
                To = mobile,
                TemplateName = "SmsTemplate_RegistrationActivation",
                CountryOptions = GetCountryOptions(country),
                Substitutions = new[]
                    {
                        new Substitution("[PLACEHOLDER_MobileConfirmationKey]", mobileConfirmationKey)
                    }
            });

        /// <inheritdoc />
        public void SendPasswordResetSms(string mobile, string passwordResetKey, string country)
            => _notificationQueue.Send(new Notification
            {
                Type = NotificationType.Sms,
                To = mobile,
                TemplateName = "SmsTemplate_PasswordReset",
                CountryOptions = GetCountryOptions(country),
                Substitutions = new[]
                    {
                        new Substitution("[PLACEHOLDER_PasswordResetKey]", passwordResetKey)
                    }
            });

        /// <inheritdoc />
        public void SendPasswordRecreationConfirmationSms(string mobile, string country)
            => _notificationQueue.Send(new Notification
            {
                Type = NotificationType.Sms,
                To = mobile,
                TemplateName = "SmsTemplate_PasswordRecreationConfirmation",
                CountryOptions = GetCountryOptions(country),
                Substitutions = new[]
                {
                    new Substitution("[PLACEHOLDER_UserId]", mobile)
                }
            });

        /// <inheritdoc />
        public void SendSubscriptionBillingWarningEmail(string email, string subscriptionPlanTitle, string paymentAmount, string country) 
            => _notificationQueue.Send(new Notification
            {
                Type = NotificationType.Email,
                To = email,
                TemplateName = "EmailTemplate_SubscriptionBillingWarning",
                CountryOptions = GetCountryOptions(country),
                Substitutions = new[]
                {
                    new Substitution("[PLACEHOLDER_SubscriptionTitle]", subscriptionPlanTitle),
                    new Substitution("[PLACEHOLDER_PaymentAmount]", paymentAmount)
                }
            });

        /// <inheritdoc />
        public void SendSubscriptionExpiredEmail(string email, string subscriptionPlanTitle, string paymentAmount, string country)
            => _notificationQueue.Send(new Notification
            {
                Type = NotificationType.Email,
                To = email,
                TemplateName = "EmailTemplate_SubscriptionExpired",
                CountryOptions = GetCountryOptions(country),
                Substitutions = new[]
                {
                    new Substitution("[PLACEHOLDER_SubscriptionTitle]", subscriptionPlanTitle),
                    new Substitution("[PLACEHOLDER_PaymentAmount]", paymentAmount)
                }
            });

        /// <inheritdoc />
        public void SendPendingPaymentFailureEmail(string email, string subscriptionPlanTitle, string paymentAmount, string country)
            => _notificationQueue.Send(new Notification
            {
                Type = NotificationType.Email,
                To = email,
                TemplateName = "EmailTemplate_PendingPaymentFailure",
                CountryOptions = GetCountryOptions(country),
                Substitutions = new[]
                {
                    new Substitution("[PLACEHOLDER_SubscriptionTitle]", subscriptionPlanTitle),
                    new Substitution("[PLACEHOLDER_PaymentAmount]", paymentAmount)
                }
            });

        public void SendSubscriptionExpiryNotificationSms(string mobile, string subscriptionPlanTitle, DateTime? subscriptionEnd, string country, SubscriptionExpiryNotificationDay day) =>
            _notificationQueue.Send(new Notification
            {
                Type = NotificationType.Sms,
                To = mobile,
                TemplateName = DeriveTemplateNameFromDay(day),
                CountryOptions = GetCountryOptions(country),
                Substitutions = new[]
                {
                    new Substitution("[PLACEHOLDER_SubscriptionTitle]", subscriptionPlanTitle),
                    new Substitution("[PLACEHOLDER_SubscriptionEndDate]", subscriptionEnd?.ToString("MMM dd, yyyy"))
                }
            });

        /// <inheritdoc />
        public void SendSubscriptionCancellationEmail(string email, string subscriptionPlanTitle, string subscriptionEndDate, string country)
            => _notificationQueue.Send(new Notification
            {
                Type = NotificationType.Email,
                To = email,
                TemplateName = "EmailTemplate_SubscriptionCancellation",
                CountryOptions = GetCountryOptions(country),
                Substitutions = new[]
                {
                    new Substitution("[PLACEHOLDER_SubscriptionTitle]", subscriptionPlanTitle),
                    new Substitution("[PLACEHOLDER_SubscriptionEndDate]", subscriptionEndDate)
                }
            });

        /// <inheritdoc />
        public void SendSubscriptionConfirmationEmail(string email, string subscriptionPlanTitle, DateTime? subscriptionEnd, string country, string promotionalText)
            => _notificationQueue.Send(new Notification
            {
                Type = NotificationType.Email,
                To = email,
                TemplateName = "EmailTemplate_SubscriptionConfirmation",
                CountryOptions = GetCountryOptions(country),
                Substitutions = new[]
                {
                    new Substitution("[PLACEHOLDER_SubscriptionTitle]", subscriptionPlanTitle),
                    new Substitution("[PLACEHOLDER_SubscriptionEndDate]", subscriptionEnd?.ToString("MMM dd, yyyy")),
                    new Substitution("[PLACEHOLDER_PromoText]", promotionalText)
                }
            });

        /// <inheritdoc />
        public void SendSubscriptionRenewalEmail(string email, string subscriptionPlanTitle, DateTime? subscriptionEnd, string country, string promotionalText)
            => _notificationQueue.Send(new Notification
            {
                Type = NotificationType.Email,
                To = email,
                TemplateName = "EmailTemplate_SubscriptionRenewal",
                CountryOptions = GetCountryOptions(country),
                Substitutions = new[]
                {
                    new Substitution("[PLACEHOLDER_SubscriptionTitle]", subscriptionPlanTitle),
                    new Substitution("[PLACEHOLDER_SubscriptionEndDate]", subscriptionEnd?.ToString("MMM dd, yyyy")),
                    new Substitution("[PLACEHOLDER_PromoText]", promotionalText)
                }
            });

        /// <inheritdoc />
        public void SendSubscriptionFailureEmail(string email, string subscriptionPlanTitle, string country)
            => _notificationQueue.Send(new Notification
            {
                Type = NotificationType.Email,
                To = email,
                TemplateName = "EmailTemplate_SubscriptionFailure",
                CountryOptions = GetCountryOptions(country),
                Substitutions = new[]
                {
                    new Substitution("[PLACEHOLDER_SubscriptionTitle]", subscriptionPlanTitle)
                }
            });

        /// <inheritdoc />
        public void SendSubscriptionConfirmationSms(string mobile, string subscriptionPlanTitle, DateTime? subscriptionEnd, string country, string promotionalText)
            => _notificationQueue.Send(new Notification
            {
                Type = NotificationType.Sms,
                To = mobile,
                TemplateName = "SmsTemplate_SubscriptionConfirmation",
                CountryOptions = GetCountryOptions(country),
                Substitutions = new[]
                {
                    new Substitution("[PLACEHOLDER_SubscriptionTitle]", subscriptionPlanTitle),
                    new Substitution("[PLACEHOLDER_SubscriptionEndDate]", subscriptionEnd?.ToString("MMM dd, yyyy")),
                    new Substitution("[PLACEHOLDER_PromoText]", promotionalText)
                }
            });

        /// <inheritdoc />
        public void SendSubscriptionRenewalSms(string mobile, string subscriptionPlanTitle, DateTime? subscriptionEnd, string country, string promotionalText, string subscriptionPlanDuration)
            => _notificationQueue.Send(new Notification
            {
                Type = NotificationType.Sms,
                To = mobile,
                TemplateName = "SmsTemplate_SubscriptionRenewal",
                CountryOptions = GetCountryOptions(country),
                Substitutions = new[]
                {
                    new Substitution("[PLACEHOLDER_SubscriptionTitle]", subscriptionPlanTitle),
                    new Substitution("[PLACEHOLDER_SubscriptionEndDate]", subscriptionEnd?.ToString("MMM dd, yyyy").ToUpper()),
                    new Substitution("[PLACEHOLDER_SubscriptionPlanDuration]", subscriptionPlanDuration), 
                }
            });

        /// <inheritdoc />
        public void SendSubscriptionFailureSms(string mobile, string subscriptionPlanTitle, string country)
            => _notificationQueue.Send(new Notification
            {
                Type = NotificationType.Sms,
                To = mobile,
                TemplateName = "SmsTemplate_SubscriptionFailure",
                CountryOptions = GetCountryOptions(country),
                Substitutions = new[]
                {
                    new Substitution("[PLACEHOLDER_SubscriptionTitle]", subscriptionPlanTitle)
                }
            });

        public void SendRecurringSubscriptionRenewalInfoSms(string mobile, string subscriptionPlanTitle, DateTime? subscriptionEnd, string country, string subscriptionPlanDuration)
        {
            _notificationQueue.Send(new Notification
            {
                Type = NotificationType.Sms,
                To = mobile,
                TemplateName = "SmsTemplate_Subscription_RenewalInfo_Recurring",
                CountryOptions = GetCountryOptions(country),
                Substitutions = new[]
                {
                    new Substitution("[PLACEHOLDER_SubscriptionTitle]", subscriptionPlanTitle),
                    new Substitution("[PLACEHOLDER_SubscriptionEndDate]", subscriptionEnd?.ToString("MMM dd, yyyy").ToUpper()),
                    new Substitution("[PLACEHOLDER_SubscriptionPlanDuration]", subscriptionPlanDuration), 
                }
            });
        }

        public void SendNonRecurringSubscriptionRenewalInfoSms(string mobile, string subscriptionPlanTitle, DateTime? subscriptionEnd, string country, string subscriptionPlanDuration)
        {
            _notificationQueue.Send(new Notification
            {
                Type = NotificationType.Sms,
                To = mobile,
                TemplateName = "SmsTemplate_Subscription_RenewalInfo_NonRecurring",
                CountryOptions = GetCountryOptions(country),
                Substitutions = new[]
                {
                    new Substitution("[PLACEHOLDER_SubscriptionTitle]", subscriptionPlanTitle),
                    new Substitution("[PLACEHOLDER_SubscriptionEndDate]", subscriptionEnd?.ToString("MMM dd, yyyy").ToUpper()),
                    new Substitution("[PLACEHOLDER_SubscriptionPlanDuration]", subscriptionPlanDuration), 
                }
            });
        }

        /// <inheritdoc />
        public void SendPurchaseConfirmationEmail(string email, string assetTitle, string country, string promotionalText) 
            => _notificationQueue.Send(new Notification
            {
                Type = NotificationType.Email,
                To = email,
                TemplateName = "EmailTemplate_PurchaseConfirmation",
                CountryOptions = GetCountryOptions(country),
                Substitutions = new[]
                {
                    new Substitution("[PLACEHOLDER_PurchaseTitle]", assetTitle),
                    new Substitution("[PLACEHOLDER_PromoText]", promotionalText), 
                }
            });

        private NotificationCountryOptions GetCountryOptions(string country)
            => string.IsNullOrEmpty(country) ? null : _countryOptionsProvider?.OptionsForCountry(country) ?? new NotificationCountryOptions { Country = country };
        
        private string DeriveTemplateNameFromDay(SubscriptionExpiryNotificationDay day)
        {
            switch (day)
            {
                case SubscriptionExpiryNotificationDay.FiveDaysAgo:
                    return "SmsTemplate_SubscriptionExpiry_After";
                case SubscriptionExpiryNotificationDay.TwoDaysAgo:
                    return "SmsTemplate_SubscriptionExpiry_After";
                case SubscriptionExpiryNotificationDay.OneDayAgo:
                    return "SmsTemplate_SubscriptionExpiry_After";
                case SubscriptionExpiryNotificationDay.Today:
                    return "SmsTemplate_SubscriptionExpiry";
                case SubscriptionExpiryNotificationDay.InOneDay:
                    return "SmsTemplate_SubscriptionExpiry_Before1";
                case SubscriptionExpiryNotificationDay.InTwoDays:
                    return "SmsTemplate_SubscriptionExpiry_Before2";
                default:
                    throw new ArgumentOutOfRangeException(nameof(day), day, $"Template name couldn't be derived from {day}.");
            }
        }
    }
}
