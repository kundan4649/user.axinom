using System.Collections.Generic;

namespace z5.ms.common.notifications
{
    /// <summary>An email notification to be sent with the notification client</summary>
    public class Notification
    {
        /// <summary>Type of notification: email or sms</summary>
        public NotificationType Type { get; set; }

        /// <summary>Destination email address / mobile number</summary>
        public string To { get; set; }

        /// <summary>Name of the template to use. Templates are loaded form resources</summary>
        public string TemplateName { get; set; }

        /// <summary>Notification sender details</summary>
        public NotificationCountryOptions CountryOptions { get; set; }

        /// <summary>Replacement values for placeholders defined in the email template</summary>
        public IEnumerable<Substitution> Substitutions { get; set; }

        public string TranslatedTemplateName
            => string.IsNullOrWhiteSpace(CountryOptions?.Country) ? TemplateName : $"{TemplateName}_{CountryOptions?.Country.ToUpperInvariant()}";

        /// <inheritdoc />
        public override string ToString() => $"{Type}[To:{To},TemplateName:{TranslatedTemplateName}]";
    }

    /// <summary>Notification type: email or sms</summary>
    public enum NotificationType
    {
        /// <summary>Notification will be sent by email</summary>
        Email,

        /// <summary>Notification will be sent by sms</summary>
        Sms
    }
}