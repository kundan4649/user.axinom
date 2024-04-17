using System;
using Microsoft.Extensions.Options;
using z5.ms.common.abstractions;
using z5.ms.common.notifications;

namespace z5.ms.user.serverless.azure.reminders
{
    internal class ReminderSender
    {
        private readonly IPublisher<Notification> _queue;
        private readonly NotificationCountryOptionsProvider _countryOptions;

        public ReminderSender(IPublisher<Notification> queue, NotificationOptions options)
        {
            _queue = queue;
            _countryOptions = new NotificationCountryOptionsProvider(new OptionsWrapper<NotificationOptions>(options));
        }

        /// <summary>Send epg reminder email</summary>
        public void SendEpgReminderEmail(EpgReminder epgReminder)
        {
            _queue.Publish(new Notification
            {
                Type = NotificationType.Email,
                To = epgReminder.User.Email,
                TemplateName = "EmailTemplate_SendReminders",
                CountryOptions = GetCountryOptions(epgReminder.User.RegistrationCountry),
                Substitutions = new[]
                {
                    new Substitution("[PLACEHOLDER_UserName]", epgReminder.User.FirstName),
                    new Substitution("[PLACEHOLDER_EpgTitle]", epgReminder.Epg.Title),
                    new Substitution("[PLACEHOLDER_RemainingMinutes]", ((int)epgReminder.Epg.StartTime.Subtract(DateTime.UtcNow).TotalMinutes).ToString()),
                    new Substitution("[PLACEHOLDER_UtcTime]", epgReminder.Epg.StartTime.ToUniversalTime().ToString("dd.MM.yyyy HH:mm:ss zzz")),
                    new Substitution("[PLACEHOLDER_ChannelName]", epgReminder.Epg.ChannelName)
                }
            });
        }

        /// <summary>Send epg reminder sms</summary>
        public void SendEpgReminderSms(EpgReminder epgReminder)
        {
            _queue.Publish(new Notification
            {
                Type = NotificationType.Sms,
                To = epgReminder.User.Mobile,
                TemplateName = "SmsTemplate_SendReminders",
                CountryOptions = GetCountryOptions(epgReminder.User.RegistrationCountry),
                Substitutions = new[]
                {
                    new Substitution("[PLACEHOLDER_UserName]", epgReminder.User.FirstName),
                    new Substitution("[PLACEHOLDER_EpgTitle]", epgReminder.Epg.Title),
                    new Substitution("[PLACEHOLDER_RemainingMinutes]", ((int)epgReminder.Epg.StartTime.Subtract(DateTime.UtcNow).TotalMinutes).ToString()),
                    new Substitution("[PLACEHOLDER_UtcTime]", epgReminder.Epg.StartTime.ToUniversalTime().ToString("dd.MM.yyyy HH:mm:ss zzz")),
                    new Substitution("[PLACEHOLDER_ChannelName]", epgReminder.Epg.ChannelName)
                }
            });
        }

        private NotificationCountryOptions GetCountryOptions(string country)
            => string.IsNullOrEmpty(country) ? null : _countryOptions?.OptionsForCountry(country) ?? new NotificationCountryOptions { Country = country };

    }
}
