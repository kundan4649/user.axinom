using System.Threading;
using System.Threading.Tasks;
using z5.ms.common.infrastructure.azure.serverless;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using z5.ms.common.notifications;
using z5.ms.user.serverless.azure.notifications;

namespace z5.ms.user.serverless.azure
{
    public static class NotficationFunctions
    {
        /// <summary>Prepares the notification message and sends it to email/sms sending queue</summary>
        [FunctionName("PrepareNotification")]
        public static async Task PrepareNotification(
            [QueueTrigger("notifications")]Notification notification,
            [Queue("notifications-email")] IAsyncCollector<NotificationMessage> emailQueue,
            [Queue("notifications-sms")] IAsyncCollector<NotificationMessage> smsQueue,
            ILogger rawlog, CancellationToken token)
        {
            var log = rawlog.ToElasticLogger("NotficationFunctions", token);
            var config = AzureHelpers.Configure<NotificationOptions>();

            switch (notification.Type)
            {
                case NotificationType.Email:
                    await TemplateFunction.Handle<EmailTemplate>(config, notification, emailQueue.ToIPublisher(), log); break;
                case NotificationType.Sms:
                    await TemplateFunction.Handle<SmsTemplate>(config, notification, smsQueue.ToIPublisher(), log); break;
            }
        }

        /// <summary>Sends email to smtp server</summary>
        [FunctionName("SendEmail")]
        public static async Task SendEmail(
            [QueueTrigger("notifications-email")]NotificationMessage notification,
            ILogger rawlog, CancellationToken token)
        {
            var log = rawlog.ToElasticLogger("NotficationFunctions", token);
            var config = AzureHelpers.Configure<NotificationOptions>();

            await EmailFunction.Handle(notification, config, log);
        }

        /// <summary>Sends email to sms server</summary>
        [FunctionName("SendSms")]
        public static async Task SendSms(
            [QueueTrigger("notifications-sms")]NotificationMessage notification,
            ILogger rawlog, CancellationToken token)
        {
            var log = rawlog.ToElasticLogger("NotficationFunctions", token);
            var config = AzureHelpers.Configure<NotificationOptions>();

            await SmsFunction.Handle(notification, config, log);
        }
    }
}