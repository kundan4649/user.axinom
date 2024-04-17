using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using z5.ms.common.abstractions;
using z5.ms.common.notifications;

namespace z5.ms.user.serverless.azure.notifications
{
    public static class TemplateFunction
    {
        public static async Task Handle<TTemplate>(NotificationOptions config, Notification notification, IPublisher<NotificationMessage> queue, ILogger log)
            where TTemplate : NotificationTemplate
        {
            var template = await NotificationHelpers
                               .GetXmlTemplate<TTemplate>(config.NotificationStorageConnection, notification.TranslatedTemplateName, null, log) ??
                           await NotificationHelpers
                               .GetXmlTemplate<TTemplate>(config.NotificationStorageConnection, notification.TemplateName, null, log);

            if (template == null)
            {
                log.LogError($"Template {notification.TemplateName} not found");
                return;
            }
            try
            {
                var userid = notification.Substitutions.ToList().Any(x => x.Key == "[PLACEHOLDER_UserId]") ? notification.Substitutions.ToList().Where(x => x.Key == "[PLACEHOLDER_UserId]").Select(x => x.Value).FirstOrDefault() : string.Empty;
                if (!string.IsNullOrWhiteSpace(userid))
                {
                    var result = await PostNotification.Postb2bNotificationData(Guid.Parse(userid), notification, config.b2bApiforNotification);
                    if (!result.Success)
                    {
                        log.LogError($"Email Verification API with Parameters {userid}, {notification.To},{notification.TemplateName},{notification.CountryOptions.Country} failed due to {result.Error.Message} ");
                    }
                }
            }
            catch (Exception ex)
            {
                log.LogError($"Error occured while sending request to b2b verification API {ex.Message}");
            }


            var body = notification.Substitutions.AddConfigSettingSubstitutions(config, notification.CountryOptions)
                .Render(template.Body);

            await queue.Publish(new NotificationMessage
            {
                To = notification.To,
                From = notification.CountryOptions?.EmailFromAddress,
                Subject = template.Subject,
                Body = body
            });
        }
    }
}