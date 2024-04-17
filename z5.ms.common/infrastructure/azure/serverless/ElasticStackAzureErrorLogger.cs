using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using z5.ms.common.abstractions;
using z5.ms.common.extensions;
using z5.ms.common.infrastructure.logging;

namespace z5.ms.common.infrastructure.azure.serverless
{
    /// <summary>Configuration for ElasticStackAzureErrorLogger</summary>
    public class ElasticStackAzureErrorLoggerConfiguration
    {
        /// <summary>Azure function storage connection</summary>
        public string AzureWebJobsStorage { get; set; }

        /// <summary>Elastic stack configuration</summary>
        public ElasticStackLogClientConfiguration ElasticStackLogClientConfiguration { get; set; }
    }

    /// <summary>Parse azure error logs and post to an elastic stack instance</summary>
    public static class ElasticStackAzureErrorLogger
    {
        /// <summary>Entry point</summary>
        /// <param name="config"></param>
        /// <param name="statusTable"></param>
        /// <param name="log"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public static async Task TransferLogEntries(ElasticStackAzureErrorLoggerConfiguration config, CloudTable statusTable, ILogger log, CancellationToken token)
        {
            if (string.IsNullOrEmpty(config.ElasticStackLogClientConfiguration.ElasticStackUrl))
            {
                log.LogWarning("Elastic stack url is not defined. Exiting");
                return;
            }

            // table name increments every month. We need to calculate the table name and we may need to pick entries from 2 tables
            var status = await LoadStatus(statusTable);
            var now = DateTime.UtcNow;

            var tableName1 = $"AzureWebJobsHostLogs{status.LastLogTimestamp.Year}{status.LastLogTimestamp.Month:D2}";
            var tableName2 = $"AzureWebJobsHostLogs{now.Year}{now.Month:D2}";

            var result = await TransferLogEntriesInTable(tableName1, config, status, statusTable, log, token);

            if (tableName1 != tableName2 && result.Success && !token.IsCancellationRequested)
            {
                status = await LoadStatus(statusTable);
                await TransferLogEntriesInTable(tableName2, config, status, statusTable, log, token);
            }
        }

        private static async Task<Result<Success>> TransferLogEntriesInTable(string tableName, ElasticStackAzureErrorLoggerConfiguration config, LogMonitorStatus status, CloudTable statusTable, ILogger log, CancellationToken token)
        {
            var storageAccount = CloudStorageAccount.Parse(config.AzureWebJobsStorage);
            var tableClient = storageAccount.CreateCloudTableClient();
            var table = tableClient.GetTableReference(tableName);
            if (!await table.ExistsAsync())
            {
                log.LogWarning($"Table {tableName} does not exist.");
                return new Result<Success>();
            }
            log.LogInformation($"Querying table {tableName}.");

            // query to select log entries since last run
            var where = $@"
                ({nameof(AzureFunctionLogEntry.Timestamp)} ge datetime'{status.LastLogTimestamp.ToZuluDateTimeString()}') 
                and ({nameof(AzureFunctionLogEntry.RowKey)} ne '{status.LastLogRowKey}') 
                and (({nameof(AzureFunctionLogEntry.LogOutput)} ne '') or ({nameof(AzureFunctionLogEntry.ErrorDetails)} ne ''))
                and ({nameof(AzureFunctionLogEntry.FunctionName)} ne 'ElasticStackErrorLogger') ";

            var query = new TableQuery<AzureFunctionLogEntry>().Where(where);
            TableContinuationToken continuationToken = null;
            do 
            {
                var queryResults = await table.ExecuteQuerySegmentedAsync(query, continuationToken);
                continuationToken = queryResults.ContinuationToken;

                if (!queryResults.Results.Any()) break;

                if (token.IsCancellationRequested)
                {
                    log.LogInformation("Cancellation requested.. exiting");
                    return new Result<Success>();
                }

                var result = await PostErrorsToElasticAndUpdateStatus(config, statusTable, log, queryResults);
                if (!result.Success)
                    return result;

                if (token.IsCancellationRequested)
                {
                    log.LogInformation("Cancellation requested.. exiting");
                    return new Result<Success>();
                }
            }
            while (continuationToken != null);

            log.LogInformation($"Log entries transferred from table {tableName}.");
            return new Result<Success>();
        }

        private static async Task<Result<Success>> PostErrorsToElasticAndUpdateStatus(ElasticStackAzureErrorLoggerConfiguration config, CloudTable statusTable, ILogger log, TableQuerySegment<AzureFunctionLogEntry> queryResults)
        {
            // filter to errors only
            var results = queryResults.Results
                .OrderBy(x => x.Timestamp)
                .ToList();

            if (!results.Any())
                return new Result<Success>();

            var errors = queryResults.Results
                .Where(x => !string.IsNullOrEmpty(x.ErrorDetails))
                .ToList();

            if (errors.Any())
            {
                var es = ElasticStackLogClient.GetInstance(config.ElasticStackLogClientConfiguration, log);
                foreach (var r in errors)
                    es.Send(ToElasticStackLogEntry(r), true);
                await es.Flush();
            }

            await SaveStatus(statusTable, results.Last().Timestamp.UtcDateTime, results.Last().RowKey, queryResults.Count());

            log.LogInformation($"{errors.Count} error logs transferred.");
            return new Result<Success>();
        }
        
        private static async Task<LogMonitorStatus> LoadStatus(CloudTable statusTable)
        {
            var result = await statusTable.ExecuteQuerySegmentedAsync(new TableQuery<LogMonitorStatus>().Take(1), new TableContinuationToken());
            return result.Results.FirstOrDefault() 
                   ?? new LogMonitorStatus(DateTime.UtcNow.AddDays(-1), "", 0); // check last 24 hours by default
        }
        
        private static async Task SaveStatus(CloudTable statusTable, DateTime lastLogTimestamp, string lastLogRowKey, int logEntryCount)
            => await statusTable.ExecuteAsync(TableOperation.InsertOrReplace(new LogMonitorStatus(lastLogTimestamp, lastLogRowKey, logEntryCount)));

        private static ElasticStackLogEntry ToElasticStackLogEntry(AzureFunctionLogEntry l)
            => new ElasticStackLogEntry
            {
                Timestamp = l.Timestamp.UtcDateTime,
                Level = Microsoft.Extensions.Logging.LogLevel.Error,
                Message = $"Error: \"{l.ErrorDetails}\" in function {l.FunctionName} called with arguments {l.ArgumentsJson} ",
                Logger = string.IsNullOrEmpty(l.FunctionName) ? l.FunctionName : "Functions infrastructure"
            };
    }

    internal class AzureFunctionLogEntry : TableEntity
    {
        public string FunctionName { get; set; }
        public string LogOutput { get; set; }
        public string ErrorDetails { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string ArgumentsJson { get; set; }
        public string TriggerReason { get; set; }
    }

    internal class LogMonitorStatus : TableEntity
    {
        public LogMonitorStatus(DateTime lastLogTimestamp, string lastLogRowKey, int logEntryCount)
        {
            RowKey = (DateTime.MaxValue.Ticks - lastLogTimestamp.Ticks).ToString("d19");
            PartitionKey = "A";
            LastLogTimestamp = lastLogTimestamp;
            LastLogRowKey = lastLogRowKey;
            LogEntryCount = logEntryCount;
        }

        public LogMonitorStatus() { }

        public DateTime LastLogTimestamp { get; set; }
        public string LastLogRowKey { get; set; }
        public int LogEntryCount { get; set; }
    }
}