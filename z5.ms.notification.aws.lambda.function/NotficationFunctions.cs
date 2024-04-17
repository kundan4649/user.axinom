using Amazon.Lambda.Core;
using Amazon.Lambda.SQSEvents;
using AWS.Lambda.Configuration.Common;
using AwsClientLibrary;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Threading;
using System.Threading.Tasks;
using z5.ms.common.abstractions;
using z5.ms.common.infrastructure.azure.serverless;
using z5.ms.common.notifications;

namespace z5.ms.notification.aws.lambda.function
{
    public class NotficationFunctions : FunctionAbstract
    {
        public async Task PrepareNotification(SQSEvent eventTrigger, ILambdaContext context)
        {
            var smsQueue = ServiceProvider.GetService<IPublisher<NotificationMessageSms>>();
            var emailQueue = ServiceProvider.GetService<IPublisher<NotificationMessageEmail>>();
            foreach (var record in eventTrigger.Records)
            {
                var notification = JsonConvert.DeserializeObject<Notification>(record.Body);
                var config = AWSHelper.Configure<NotificationOptions>();
                switch (notification.Type)
                {
                    case NotificationType.Email:
                        await TemplateFunction.Handle<EmailTemplate>(config, notification, emailQueue);
                        break;
                    case NotificationType.Sms:
                        await TemplateFunction.Handle<SmsTemplate>(config, notification, smsQueue);
                        break;
                }
            }
        }

        public async Task SendEmail(SQSEvent eventTrigger, ILambdaContext context)
        {
            foreach (var record in eventTrigger.Records)
            {
                var notification = JsonConvert.DeserializeObject<NotificationMessage>(record.Body);
                var config = AWSHelper.Configure<NotificationOptions>();
                await EmailFunction.Handle(notification, config);
            }
        }

        public async Task SendSms(SQSEvent eventTrigger, ILambdaContext context)
        {
            foreach (var record in eventTrigger.Records)
            {
                var notification = JsonConvert.DeserializeObject<NotificationMessage>(record.Body);
                var config = AWSHelper.Configure<NotificationOptions>();
                await SmsFunction.Handle(notification, config);
            }
        }
    }
}