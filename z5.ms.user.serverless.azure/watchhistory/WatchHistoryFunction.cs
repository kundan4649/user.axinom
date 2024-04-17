using System;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Dapper.FastCrud;
using Dapper.FastCrud.Configuration.StatementOptions.Builders;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using z5.ms.domain.user.datamodels;
using z5.ms.domain.user.messages;

namespace z5.ms.user.serverless.azure.watchhistory
{
    /// <summary>Configuration parameters for watch history operations</summary>
    public class WatchHistoryFunctionConfiguration
    {
        /// <summary>Connection string for user database</summary>
        public string WatchHistoryDatabaseConnection { get; set; }
    }

    /// <summary>
    /// Function implementation to make watch history operations add/update/delete
    /// </summary>
    public static class WatchHistoryFunction
    {
        /// <summary>
        /// Handler for watch history queue messages
        /// </summary>
        /// <param name="config"></param>
        /// <param name="message"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        public static async Task Handle(WatchHistoryFunctionConfiguration config, WatchHistoryMessage message, ILogger log)
        {
            try
            {
                switch (message.Type)
                {
                    case MessageType.Add:
                        await AddWatchHistory(message.Item, config.WatchHistoryDatabaseConnection, log);
                        break;
                    case MessageType.Update:
                        await UpdateWatchHistory(message.Item, config.WatchHistoryDatabaseConnection, log);
                        break;
                    case MessageType.Delete:
                        await DeleteWatchHistory(message.Item, config.WatchHistoryDatabaseConnection, log);
                        break;
                    default:
                        log.LogError("Invalid message type");
                        return;
                }
            }
            catch (Exception e)
            {
                throw new Exception($"Failed to {message.Type.ToString().ToLower()} item {JsonConvert.SerializeObject(message.Item, Formatting.None)} - Error: {e.Message}");
            }
        }

        private static async Task AddWatchHistory(WatchHistoryEntity item, string connectionString, ILogger log)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                var sw = Stopwatch.StartNew();
                var existing = await conn.FindAsync<WatchHistoryEntity>(q => q
                    .Where($"UserId = @UserId AND AssetId = @AssetId")
                    .WithParameters(new { item.UserId, item.AssetId }).OrderBy($"1").Top(1));

                if (existing.Any())
                    log.LogDebug($"Watchistory item already exists {item.UserId}/{item.AssetId}");

                await conn.InsertAsync(item);

                sw.Stop();
                log.LogDebug($"Watchistory added successfully {item.UserId}/{item.AssetId} in {sw.ElapsedMilliseconds}ms");
            }
        }

        private static async Task UpdateWatchHistory(WatchHistoryEntity item, string connectionString, ILogger log)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                var sw = Stopwatch.StartNew();
                var result = await conn.BulkUpdateAsync(item, q => q
                    .Where($"UserId = @UserId AND AssetId = @AssetId")
                    .WithParameters(new { item.UserId, item.AssetId })
                    .WithFields("Duration"));

                sw.Stop();

                if (result > 0)
                    log.LogDebug($"Watchistory updated successfully {item.UserId}/{item.AssetId} in {sw.ElapsedMilliseconds}ms");
                else
                    log.LogError($"Failed to update watch history. No item found {item.UserId}/{item.AssetId}");
            }
        }

        private static async Task DeleteWatchHistory(WatchHistoryEntity item, string connectionString, ILogger log)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                var sw = Stopwatch.StartNew();
                var result = await conn.BulkDeleteAsync<WatchHistoryEntity>(q => q
                    .Where($"UserId = @UserId AND AssetId = @AssetId")
                    .WithParameters(new { item.UserId, item.AssetId }));

                sw.Stop();
                if (result > 0)
                    log.LogDebug($"Watchistory deleted successfully {item.UserId}/{item.AssetId} in {sw.ElapsedMilliseconds}ms");
                else
                    log.LogError($"Failed to delete watch history. No item found {item.UserId}/{item.AssetId}");
            }
        }

        private static IConditionalBulkSqlStatementOptionsBuilder<T> WithFields<T>(this IConditionalBulkSqlStatementOptionsBuilder<T> builder, params string[] fieldsToUpdate)
        {
            var partialMapping = OrmConfiguration
                .GetDefaultEntityMapping<T>().Clone()
                .UpdatePropertiesExcluding(propertyMapping => propertyMapping.IsExcludedFromUpdates = true, fieldsToUpdate);

            return builder.WithEntityMappingOverride(partialMapping);
        }
    }
}
