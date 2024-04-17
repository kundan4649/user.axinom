using System;
using System.Threading;
using System.Threading.Tasks;
using z5.ms.common.helpers;
using z5.ms.common.infrastructure.azure.serverless;
using z5.ms.common.infrastructure.logging;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Table;

namespace z5.ms.user.serverless.azure
{
    public static class LogMonitorFunctions
    {
        // Send error logs from table storage to elastic stack
        [FunctionName("ElasticStackErrorLogger")]
        public static async Task ElasticStackErrorLogger(
            [TimerTrigger("0 */5 * * * *")]TimerInfo ctx,
            [Table("ErrorLogMonitorStatus")]CloudTable statusTable,
            ILogger log,
            CancellationToken token)
        {
            // timeout 20 seconds before next run
            token = token.WithTimeout(ctx.Schedule.GetNextOccurrence(DateTime.UtcNow).AddSeconds(-20));
            var configuration = AzureHelpers.Configure<ElasticStackAzureErrorLoggerConfiguration>();
            configuration.ElasticStackLogClientConfiguration = AzureHelpers.Configure<ElasticStackLogClientConfiguration>();
            await ElasticStackAzureErrorLogger.TransferLogEntries(configuration, statusTable, log, token);
        }
    }
}
