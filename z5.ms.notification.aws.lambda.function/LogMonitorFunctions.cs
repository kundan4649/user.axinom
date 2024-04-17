using Amazon.Lambda.CloudWatchEvents;
using Amazon.Lambda.Core;
using AWS.Lambda.Configuration.Common;
using AwsClientLibrary;
using System.Threading.Tasks;

namespace z5.ms.notification.aws.lambda.function
{

    public class LogMonitorFunctions : FunctionAbstract
    {
        public async Task ElasticStackErrorLogger(CloudWatchEvent<Payload> eventTrigger, ILambdaContext context)
        {
            // Not in use
            //ILogger log = ServiceProvider.GetService<ILogger>();
            //CancellationToken token = new CancellationToken();
            //token = token.WithTimeout(ctx.Schedule.GetNextOccurrence(DateTime.UtcNow).AddSeconds(-20));
            //var configuration = AWSHelper.Configure<ElasticStackAzureErrorLoggerConfiguration>();
            //configuration.ElasticStackLogClientConfiguration = AWSHelper.Configure<ElasticStackLogClientConfiguration>();
            //await ElasticStackAzureErrorLogger.TransferLogEntries(configuration, statusTable, log, token);
        }
    }
}