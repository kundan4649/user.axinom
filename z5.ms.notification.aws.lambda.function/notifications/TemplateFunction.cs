using System;
using System.Linq;
using System.Threading.Tasks;
using Amazon.S3;
using Microsoft.Extensions.Logging;
using z5.ms.common.abstractions;
using z5.ms.common.notifications;

namespace z5.ms.notification.aws.lambda.function
{
    public static class TemplateFunction
    {
        public static async Task Handle<TTemplate>(NotificationOptions config, Notification notification, IPublisher<NotificationMessageEmail> queue, ILogger log = null)
           where TTemplate : NotificationTemplate
        {
            try
            {
                IAmazonS3 amazonClient = new AmazonS3Client();
                var template = await NotificationHelpers
                                   .GetXmlTemplateFromS3<TTemplate>(amazonClient, config.TemplateS3BucketName, notification.TranslatedTemplateName, null, log) ??
                               await NotificationHelpers
                                   .GetXmlTemplateFromS3<TTemplate>(amazonClient, config.TemplateS3BucketName, notification.TemplateName, null, log);

                if (template == null)
                {
                    Console.WriteLine($"TemplateBucketName: {config.TemplateS3BucketName} Template: {notification.TemplateName} not found");
                    return;
                }

                var body = await PrepareMessageFromTemplate(config, notification, log, template);

                await queue.Publish(new NotificationMessageEmail
                {
                    To = notification.To,
                    From = notification.CountryOptions?.EmailFromAddress,
                    Subject = template.Subject,
                    Body = body
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message} Stack Trace:{ex.StackTrace}");
            }
        }

        public static async Task Handle<TTemplate>(NotificationOptions config, Notification notification, IPublisher<NotificationMessageSms> queue, ILogger log = null)
              where TTemplate : NotificationTemplate
        {
            try
            {
                IAmazonS3 amazonClient = new AmazonS3Client();
                var template = await NotificationHelpers
                                   .GetXmlTemplateFromS3<TTemplate>(amazonClient, config.TemplateS3BucketName, notification.TranslatedTemplateName, null, log) ??
                               await NotificationHelpers
                                   .GetXmlTemplateFromS3<TTemplate>(amazonClient, config.TemplateS3BucketName, notification.TemplateName, null, log);

                if (template == null)
                {
                    Console.WriteLine($"Template {notification.TemplateName} not found");
                    return;
                }

                var body = await PrepareMessageFromTemplate(config, notification, log, template);

                await queue.Publish(new NotificationMessageSms
                {
                    To = notification.To,
                    From = notification.CountryOptions?.EmailFromAddress,
                    Subject = template.Subject,
                    Body = body
                });

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message} Stack Trace:{ex.StackTrace}");
            }
        }

        public static async Task<string> PrepareMessageFromTemplate<TTemplate>(NotificationOptions config, Notification notification, ILogger log, TTemplate template)
           where TTemplate : NotificationTemplate
        {

            IAmazonS3 amazonClient = new AmazonS3Client();
            try
            {
                var userid = notification.Substitutions.ToList().Any(x => x.Key == "[PLACEHOLDER_UserId]") ? notification.Substitutions.ToList().Where(x => x.Key == "[PLACEHOLDER_UserId]").Select(x => x.Value).FirstOrDefault() : string.Empty;
                if (!string.IsNullOrWhiteSpace(userid))
                {
                    var result = await PostNotification.Postb2bNotificationData(Guid.Parse(userid), notification, config.b2bApiforNotification);
                    if (!result.Success)
                    {
                        Console.WriteLine($"Email Verification API with Parameters {userid}, {notification.To},{notification.TemplateName},{notification.CountryOptions.Country} failed due to {result.Error.Message} ");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occured while sending request to b2b verification API {ex.Message}");
            }

            return notification.Substitutions.AddConfigSettingSubstitutions(config, notification.CountryOptions)
                .Render(template.Body);

        }
    }
}