using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using z5.ms.common.infrastructure.azure.serverless;
using z5.ms.common.notifications;
using z5.ms.user.serverless.azure.reminders;

namespace z5.ms.user.serverless.azure
{
    public static class ReminderFunctions
    {
        /// <summary>Function to send epg reminders in configured period</summary>
        [FunctionName("SendReminders")]
        public static async Task SendReminders(
            [TimerTrigger("%EpgRemindersSchedule%")]TimerInfo timerInfo,
            [Queue("notifications", Connection = "NotificationQueueConnection")] IAsyncCollector<Notification> queue,
            ILogger rawlog, CancellationToken token)
        {
            var log = rawlog.ToElasticLogger("SendReminders", token);
            var functionParameters = AzureHelpers.Configure<RemindersParameters>();
            functionParameters.NotificationOptions = AzureHelpers.Configure<NotificationOptions>();

            var result = await SendRemindersFunction.Handle(functionParameters, queue.ToIPublisher(), log);

            if (result.Success)
                log.LogInformation(result.Value?.ToString() ?? "Success");
            else
                log.LogError(result.Error.ToString());
        }
    }
}