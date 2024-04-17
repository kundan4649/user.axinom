using System.Threading;
using System.Threading.Tasks;
using z5.ms.common.infrastructure.azure.serverless;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using z5.ms.domain.user.messages;
using z5.ms.user.serverless.azure.watchhistory;

namespace z5.ms.user.serverless.azure
{
    public static class WatchHistoryFunctions
    {
        /// <summary>Function to handle watchhitory queue to add/update watchhistory items</summary>
        [FunctionName("WatchHistoryOperations")]
        public static async Task WatchHistoryOperations(
            [QueueTrigger("watchhistory-operations")]WatchHistoryMessage message,
            ILogger rawlog, CancellationToken token)
        {
            var log = rawlog.ToElasticLogger("WatchHistoryOperations", token);
            var config = AzureHelpers.Configure<WatchHistoryFunctionConfiguration>();
            await WatchHistoryFunction.Handle(config, message, log);
        }
    }
}
